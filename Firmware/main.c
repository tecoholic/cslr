/*----------------------------------------------------
Title:	Real Time Customer Side Load Regulation
Author:	P.Arunmozhi
Email:	aruntheguy@gmail.com
Hardware: ATmeag32 16PU @ 1MHz Internal oscillator
Dependency: Peter Flury's LCD library for LCD usage
Software: WinAVR on Windows Vista
-----------------------------------------------------*/
#include <stdlib.h>
#include <avr/io.h>
#include <avr/pgmspace.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include "lcd.h"

#define DEVICE_ID 16

#define FIXED_MODE 0
#define DYNAMIC_MODE 1
#define DEACTIVATED 0
#define ACTIVATED 1

#define USART_BAUDRATE 2400
#define BAUD_PRESCALE (((F_CPU / (USART_BAUDRATE * 16UL))) - 1)

//setting up the initial values
volatile int limit = 100;
volatile short int mode = FIXED_MODE;
volatile unsigned int usage = 0;
unsigned short int relay_status = DEACTIVATED;
int peak = 0;

/* ------------------------------------------------------------
Function Prototypes are declared here. For the working of each 
function check the inline comments of each function
-------------------------------------------------------------*/
void initialize(void);
void check_limit(void);
void send(int value, int base);
void put_limit(int lim,int id);

/* ------------------------------------------------------------
 initialize() function set the initial settings for the device
 to start operation, which includes setting up various register
 bits and putting text on LCD. Refer inline comments for more..
--------------------------------------------------------------*/
void initialize(void)
{
	//Enable interupts
	sei();
	//Disable JTAG to use PORTC for LCD Interface
	MCUCSR |= (1<<JTD);
	MCUCSR |= (1<<JTD);//Should be written twice within 4 cycles
		
	//------------------------------------------------------------
	//Initialze ADC
	//REFS0 set AVCC as reference; 1<<ADLAR  to left adjust the data
	ADMUX |= ((1<<REFS0)|(1<<ADLAR));
	//Enable ADC, ADC Interrupt & set prescalar = 8
	ADCSRA |= ((1<<ADEN)|(1<<ADIE)|(1<<ADPS1)|(1<<ADPS0));
	//Disable free running mode
	ADCSRA &= (0<<ADATE);
	
	//-------------------------------------------------------------
	//Initialize USART
	//Enable reciever and Transmitter
	UCSRB |= (1<<RXEN)|(1<<TXEN);
	// Use 8-bit character sizes - URSEL bit set to select
	// the UCRSC register:
	UCSRC |= (1 << URSEL) | (1 << UCSZ0) | (1 << UCSZ1);
	// Load lower 8-bits of the baud rate value into the low byte
	// of the UBRR register:
	UBRRL = BAUD_PRESCALE;
	// Load upper 8-bits of the baud rate value into the high byte
	//of the UBRR register:
	UBRRH = (BAUD_PRESCALE >> 8);
	//Enable the Serial Interrupt
	UCSRB |= (1 << RXCIE);
	
	//-----------------------------------------------------------
	//Initialise the GREEN and RED Leds in PORTB 0 & 1
	DDRB |= ((1<<0)|(1<<1));
	PORTB |= (1<<1); //Set the green LED
	//Initialise the Mode switch in PORTD 2
	DDRD |= (0<<2);
	PORTD |= (1<<2); //enabling internal pull up
	//Enabling any logic change to trigger interrupt INT0
	MCUCR |= ((1<<ISC01) | (1<<ISC00));
	//Enable INT0 in GICR
	GICR |= (1<<INT0);
	//Initialise PORTA pin 2 & 3 as Outputs for Relay and Buzzer
	DDRA |= ((1<<2)|(1<<3));
	
	//----------------------------------------------------------
	//Set LCD text
	lcd_init(LCD_DISP_ON); 	//initialse  the LCD
	lcd_clrscr();			//Clear the LCD display
	lcd_puts("Limit: ");
	//convert the limit percent to Amps before displaying
	put_limit(limit,0);
	lcd_puts("\nUsage:");
	put_limit(usage,1);
	
	//------------------------------------------------------------
	// Configure timer 1 for CTC mode
	TCCR1B |= (1 << WGM12);
	// Set count for 5 sec (approx) in a prescale of 1024
	OCR1A = 4882;
	//Enable Timer Interrupt
	TIMSK |= (1 << OCIE1A);
	
}
/*-------------------------------------------------------------
The check_limit() function compares the limit value to the ADC
measurement and performs necessary action like sounding the buzzer
or activating the relay. For more refer inline comments.
-------------------------------------------------------------*/
void check_limit(void)
{
	
	if ((usage > limit)&&(mode == FIXED_MODE))
	{
		//if timer = 0 
		if ( TCNT1 == 0)
		{
			//sound buzzer
			PORTA |= (1<<3);
			//start timer for 5 sec
			TCCR1B |= ((1 << CS10) | (1 << CS12)); //Prescale of 1024
		//on time out activate relay -> ISR
		}
	}
}
/*-------------------------------------------------------------
The send(int value, int base) fucntion is used to send a number 
via the USART. It takes two arguments value -> it is the 
numerical value which has to be sent, base -> it is the base for
conversion in the itoa function. Refer itoa() for further on base
--------------------------------------------------------------*/
void send(int value, int base)
{
	char buffer[5];
	int i=0;
	itoa(value,buffer,base);
	while(buffer[i] != '\0')
	{
		while ((UCSRA & (1 << UDRE)) == 0) {};
			UDR = buffer[i];
			i++;
	}
}
/* ------------------------------------------------------------
put_limit(int limit, int id=0) function gets the integer limit 
value and converts it into suitable Amperes and outputs in the 
LCD. The id parameter is used to specify when writing the "Usage".
1-> Write "Usage". 0 -> "Limit"
-------------------------------------------------------------*/
void put_limit(int lim, int id)
{
	char buffer[4];
	itoa(lim,buffer,10);
	if(id)
	{
		lcd_gotoxy(7,1);
		lcd_puts("   ");
		lcd_gotoxy(7,1);
	}
	else
	{
		lcd_gotoxy(7,0);
		lcd_puts("   ");
		lcd_gotoxy(7,0);
	}
	
	if (lim <= 99)
	{
		lcd_puts(" ");
		lcd_putc(buffer[0]);
		lcd_puts(".");
		lcd_putc(buffer[1]);
	}
	else
	{
		lcd_putc(buffer[0]);
		lcd_putc(buffer[1]);
		lcd_puts(".");
		lcd_putc(buffer[2]);
	}
	lcd_puts(" Amps");
}

/* -------------------------------------------------------------
Main Function of the program which controls the controller
opeartion
---------------------------------------------------------------*/
int main(void)
{
    initialize();
	while(1){		
		//Perform ADC conversion and find out usage
		for(int i=0;i<50;i++)
		{
			//Start Conversion
			ADCSRA |= (1<<ADSC);
			_delay_ms(1);
		}
		// After getting the peak value convert it to useful form
		usage = (peak*100)/512;
		//Put usage on LCD
		put_limit(usage,1);
		//Check Limit Value -> Take necessary action
		check_limit();
	}
}

//Interrupt service routine for ADC conversion end
ISR(ADC_vect)
{
	//set the new peak value
	int temp;	
	temp = ADC;
	//remove offset 2.5V
	temp = temp - 512;
	//If negative invert value
	if(temp < 0)
		temp *= -1;
	if(peak < temp)
		peak = temp;
}
// ISR for Alarm timer overflow
ISR(TIMER1_COMPA_vect)
{
	// Activate Relay
	PORTA |= (1<<2);
	relay_status = ACTIVATED;
	//Switch off the buzzer
	PORTA &= (0<<3);
	//Stop the clock
	TCCR1B &= (0<< CS10);
	TCCR1B &= (0<< CS12);
	TCNT1 = 0;
}
//Interrupt service for mode change
ISR(INT0_vect)
{
	cli();
	_delay_ms(50);
	if (mode == DYNAMIC_MODE)
	{
		mode = FIXED_MODE;
		//change LED to GREEN
		PORTB &= (0<<0); //OFF RED
		PORTB |= (1<<1); //ON GREEN
	}
	else if(mode == FIXED_MODE)
	{
		mode = DYNAMIC_MODE;
		//change LED to RED
		PORTB &= (0<<1); //OFF GREEN
		PORTB |= (1<<0); //ON RED
		//if relay activated ? deactivate : do nothing
		if (relay_status == ACTIVATED )
			{
				PORTA &= (0<<2);
				relay_status = DEACTIVATED;
			}
	}
	sei();
}
//Interrupt service for Serial Byte recieved
//Note: UDR must be read to reset the ISR flag bit
ISR(USART_RXC_vect)
{
	char rByte;
	char buffer[5];
	int i=0;
	rByte = UDR;
	//lcd_putc(UDR);
	if (rByte == '1')
	{
		//send customer ID
		send(DEVICE_ID,10);
	}
	else if (rByte == '2')
	{
		//get the limit and set it. Use atoi()
		cli();
		UDR = '1';
		//Note: limit should be a 3 character string
		while(i<4)
		{
			// Do nothing until data have been recieved and is ready
			// to be read from the UDR register:
			while ((UCSRA & (1 << RXC)) == 0) {};
			buffer[i] = UDR;
			i++;
		}
		buffer[i]='\0';
		limit = atoi(buffer);
		put_limit(limit,0);
		sei();
	}
	else if (rByte == '3')
	{
		//send the mode
		if(mode == DYNAMIC_MODE)
			UDR = 'D';
		else
			UDR = 'F';
	}
	//*/
}
