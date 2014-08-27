////////////////////////////////////////////////////////////////////////////////
/* Bootloader
 * Autor: Mirosław Kardaś (2009-06-16)
 * Modyfikacje: Paweł Szramowski (2009-07-08)
 * Wersja: 2009-07-03 04:00 */
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
/* Dołączenie bibliotek */
////////////////////////////////////////////////////////////////////////////////

/* biblioteki avr-libc */
#include <stdbool.h>
#include <stdint.h>
#include <avr/boot.h>
#include <avr/io.h>
#include <avr/pgmspace.h>
#include <avr/wdt.h>
#include <util/delay.h>
//#include <util/setbaud.h>

////////////////////////////////////////////////////////////////////////////////
/* Definicje stałych programowych i makr */
////////////////////////////////////////////////////////////////////////////////

//#define UART_BAUD 57600		// tu definiujemy interesującą nas prędkość
#define __UBRR ((F_CPU+BAUD*8UL)/(16UL*BAUD)-1)  // obliczamy UBRR dla U2X=0

/* wersja bootloadera */
#define BOOTLOADER_VERSION 1

/* ustawienia UART */
#define UART_DATA_BITS 8
#define UART_PARITY 'n'
#define UART_STOP_BITS 1

/* czas przesyłania jednego znaku przez UART [us] */
#if UART_PARITY == 'n'
    #define UART_BYTE_TIME \
        ( 1000000UL / ( BAUD / ( 1 + UART_DATA_BITS + UART_STOP_BITS ) ) )
#elif ( UART_PARITY == 'e' ) || ( UART_PARITY == 'o' )
    #define UART_BYTE_TIME \
        ( 1000000UL / ( BAUD / ( 1 + UART_DATA_BITS + 1 + UART_STOP_BITS ) ) )
#endif

/* podstawienie nazwa rejestrów UART */
#ifndef UCSRA
    #define UCSRA UCSR0A
#endif
#ifndef UCSRB
    #define UCSRB UCSR0B
#endif
#ifndef UCSRC
    #define UCSRC UCSR0C
#endif
#ifndef UBRRL
    #define UBRRL UBRR0L
#endif
#ifndef UBRRH
    #define UBRRH UBRR0H
#endif
#ifndef UDR
    #define UDR UDR0
#endif

/* podstawienie nazw bitów w rejestrze UCSRA */
#ifndef RXC
    #define RXC RXC0
#endif
#ifndef TXC
    #define TXC TXC0
#endif
#ifndef UDRE
    #define UDRE UDRE0
#endif
#ifndef FE
    #define FE FE0
#endif
#ifndef DOR
    #define DOR DOR0
#endif
#ifndef UPE
    #define UPE UPE0
#endif
#ifndef U2X
    #define U2X U2X0
#endif
#ifndef MPCM
    #define MPCM MPCM0
#endif

/* podstawienie nazw bitów w rejestrze UCSRB */
#ifndef RXCIE
    #define RXCIE RXCIE0
#endif
#ifndef TXCIE
    #define TXCIE TXCIE0
#endif
#ifndef UDRIE
    #define UDRIE UDRIE0
#endif
#ifndef RXEN
    #define RXEN RXEN0
#endif
#ifndef TXEN
    #define TXEN TXEN0
#endif
#ifndef UCSZ2
    #define UCSZ2 UCSZ02
#endif
#ifndef RXB8
    #define RXB8 RXB80
#endif
#ifndef TXB8
    #define TXB8 TXB80
#endif

/* podstawienie nazw bitów w rejestrze UCSRC */
#ifndef UMSEL
    #ifdef UMSEL0
        #define UMSEL UMSEL0
    #else
        #define UMSEL UMSEL00
        #define UMSEL0 UMSEL00
        #define UMSEL1 UMSEL01
    #endif
#endif
#ifndef UPM1
    #define UPM1 UPM01
#endif
#ifndef UPM0
    #define UPM0 UPM00
#endif
#ifndef USBS
    #define USBS USBS0
#endif
#ifndef UCSZ1
    #define UCSZ1 UCSZ01
#endif
#ifndef UCSZ0
    #define UCSZ0 UCSZ00
#endif
#ifndef UCPOL
    #define UCPOL UCPOL0
#endif

/* ustawienia bitów konfiguracyjnych UART */
#if UART_DATA_BITS == 5
    #define UCSRB_DATA_BITS ( 0 << UCSZ2 )
    #define UCSRC_DATA_BITS ( 0 << UCSZ1 | 0 << UCSZ0 )
#elif UART_DATA_BITS == 6
    #define UCSRB_DATA_BITS ( 0 << UCSZ2 )
    #define UCSRC_DATA_BITS ( 0 << UCSZ1 | 1 << UCSZ0 )
#elif UART_DATA_BITS == 7
    #define UCSRB_DATA_BITS ( 0 << UCSZ2 )
    #define UCSRC_DATA_BITS ( 1 << UCSZ1 | 0 << UCSZ0 )
#elif UART_DATA_BITS == 8
    #define UCSRB_DATA_BITS ( 0 << UCSZ2 )
    #define UCSRC_DATA_BITS ( 1 << UCSZ1 | 1 << UCSZ0 )
#elif UART_DATA_BITS == 9
    #define UCSRB_DATA_BITS ( 1 << UCSZ2 )
    #define UCSRC_DATA_BITS ( 1 << UCSZ1 | 1 << UCSZ0 )
#endif

#if UART_PARITY == 'n'
    #define UCSRC_PARITY ( 0 << UPM1 | 0 << UPM0 )
#elif UART_PARITY == 'e'
    #define UCSRC_PARITY ( 1 << UPM1 | 0 << UPM0 )
#elif UART_PARITY == 'o'
    #define UCSRC_PARITY ( 1 << UPM1 | 1 << UPM0 )
#endif

#if UART_STOP_BITS == 1
    #define UCSRC_STOP_BITS ( 0 << USBS )
#elif UART_STOP_BITS == 2
    #define UCSRC_STOP_BITS ( 1 << USBS )
#endif

/* makro zamieniające podany argument na łańcuch znaków */
#define TOSTRING( x ) STRINGIFY( x )
#define STRINGIFY( x ) #x

////////////////////////////////////////////////////////////////////////////////
/* Deklaracje funkcji statycznych */
////////////////////////////////////////////////////////////////////////////////

static void UART_TX_Byte(
    const uint8_t Data );

static void UART_TX_String_P(
    const char *String_Ptr );

static uint16_t _UART_RX_Wait(
    uint16_t Timeout );

/* Makro przeliczające argument w milisekundach na przyjmowaną przez funkcję 
 * _UART_RX_Wait wielokrotność czasu przesyłania jednego znaku. */
//#define UART_RX_Wait( x ) _UART_RX_Wait( ( ( x ) > UART_BYTE_TIME ) ? \
//    ( ( ( x ) * 1000UL ) / UART_BYTE_TIME ) : 1 )
    
#define UART_RX_Wait( x ) _UART_RX_Wait( ( x * 1000UL ) / UART_BYTE_TIME )    

////////////////////////////////////////////////////////////////////////////////
/* Definicje funkcji */
////////////////////////////////////////////////////////////////////////////////

static void __vectors( 
    void )
    __attribute__ (( section( ".vectors" ), naked, used ));
static void __vectors( 
    void )
{
    /* skok do sekcji .init2 (konieczny ze względu na umieszczanie stałych z 
     * pamięci programu pomiędzy sekcjami .vectors a .init0, a więc na samym 
     * początku programu w przypadku wykorzystania opcji -nostartfiles */
    asm (
        "rjmp __init2" "\n\t"
        : : );
}

static void __init2( 
    void )
    __attribute__ (( section( ".init2" ), naked, used ));
static void __init2( 
    void )
{
    /* inicjalizacja wskaźnika stosu, rejestru z zerem i rejestru flag */
	asm volatile (
		"out __SP_L__, %A0" "\n\t"
		"out __SP_H__, %B0" "\n\t"
		"clr __zero_reg__" "\n\t"
		"out __SREG__, __zero_reg__" "\n\t"
        : : "r"( ( uint16_t )( RAMEND ) ) );
}

//#define WDIF

#ifdef WDIF
    static void __init3( 
        void )
        __attribute__ (( section( ".init3" ), naked, used ));
    static void __init3( 
        void )
    {
        /* wyłączenie watchdoga (w tych mikrokontrolerach, w których watchdog 
         * ma możliwość generowania przerwania pozostaje on też aktywny po 
         * resecie) */
        MCUSR = 0;
        _WD_CONTROL_REG = 1 << _WD_CHANGE_BIT | 1 << WDE;
        _WD_CONTROL_REG = 0;
    }
#endif

static void __init9( 
    void )
    __attribute__ (( section( ".init9" ), naked, used ));
static void __init9( 
    void )
{
    /* skok do funkcji main */
	asm (
		"rjmp main" "\n\t"
        : : );
}

int main( 
    void )
    __attribute__ (( noreturn ));
int main( 
    void )
{
    /* konfiguracja i włączenie interfejsu UART */
    UBRRH = __UBRR>>8;//UBRRH_VALUE;
    UBRRL = __UBRR;//UBRRL_VALUE;
    /*
    #if USE_2X
        UCSRA = 1 << U2X;
    #else
        UCSRA = 0;
    #endif
    */
    #ifdef URSEL
        UCSRC = 1 << URSEL | UCSRC_DATA_BITS | UCSRC_PARITY | UCSRC_STOP_BITS;
    #else
        UCSRC = UCSRC_DATA_BITS | UCSRC_PARITY | UCSRC_STOP_BITS;
    #endif
    UCSRB = 1 << RXEN | 1 << TXEN | UCSRB_DATA_BITS;
    
    /* deklaracja zmiennych */
    uint16_t Received_Char;
    
    /* wysyłanie znaku '?' do odebrania znaku 'u' lub upłynięcia czasu 
     * oczekiwania */
	for ( uint8_t Retries_Left = BOOT_WAIT * 10; Retries_Left; --Retries_Left )
    {
		UART_TX_Byte( '?' );
		_delay_ms(100);
        
        if ( ( uint8_t )UART_RX_Wait( 100 ) == 'u' )
        {
            /* definicja zmiennych */
            bool Is_Comm_Lost = false;
            
            while ( 1 )
            {
                /* odebranie znaku */
                uint16_t Received_Char = UART_RX_Wait( 1000 );
                
                if ( ( uint8_t )Received_Char == 'i' )
                {
                    /* jeśli odebrano znak 'i', to wysłanie ciągu znaków 
                     * identyfikujących mikrokontroler i bootloader */
                    UART_TX_String_P( PSTR( "\r\n" "&" 
                        TOSTRING( SPM_PAGESIZE ) "," 
                        TOSTRING( BLS_START ) "," 
                        TOSTRING( MCU ) "," 
                        TOSTRING( XTAL ) "," 
                        TOSTRING( BOOTLOADER_VERSION )
                        "*" "\r\n" ) );
                }
                else if ( ( uint8_t )Received_Char == 'w' )
                {
                    /* odczekanie do zakończenia operacji na pamięci EEPROM 
                     * i Flash */
                    eeprom_busy_wait();
                    boot_spm_busy_wait();
                    
                    /* kasowanie całej pamięci programu */
                    for ( uint32_t Page_Address = 0; Page_Address < BLS_START; 
                        Page_Address += SPM_PAGESIZE )
                    {
                        /* skasowanie strony pamięci */
                        boot_page_erase( Page_Address );
                        boot_spm_busy_wait();
                    }
                    
                    /* programowanie pamięci programu */
                    for ( uint32_t Page_Address = 0; Page_Address < BLS_START; 
                        Page_Address += SPM_PAGESIZE )
                    {
                        /* wysłanie znaku '@' */
                        UART_TX_Byte( '@' );
                        
                        /* jeśli odebrano zero lub upłynął czas oczekiwania na 
                         * odpowiedź, to zakończenie pracy bootloadera */
                        Received_Char = UART_RX_Wait( 1000 );
                        if ( !Received_Char || Received_Char > 0xff )
                            break;
                        
                        /* zapełnienie bufora strony */
                        for ( uint16_t Byte_Address = 0; 
                            Byte_Address < SPM_PAGESIZE; Byte_Address += 2 )
                        {
                            /* próba odebrania młodszego bajtu słowa z pamięci 
                             * programu */
                            if ( ( Received_Char = UART_RX_Wait( 1000 ) ) 
                                > 0xff )
                            {
                                Is_Comm_Lost = true;
                                break;
                            }
                            
                            uint16_t Instruction = Received_Char;
                            
                            /* próba odebrania starszego bajtu słowa z pamięci 
                             * programu */
                            if ( ( Received_Char = UART_RX_Wait( 1000 ) ) 
                                > 0xff )
                            {
                                Is_Comm_Lost = true;
                                break;
                            }
                            
                            Instruction += Received_Char << 8;
                            
                            /* zapisanie instrukcji do bufora */
                            boot_page_fill( Byte_Address, Instruction );
                        }
                        
                        if ( Is_Comm_Lost )
                            break;
                        
                        /* zapisanie strony pamięci */
                        boot_page_write( Page_Address );
                        boot_spm_busy_wait();
                    }
                    
                    /* odblokowanie sekcji Read-While-Write */
                    boot_rww_enable();
                    
                    /* zakończenie pracy bootloadera */
                    break;
                }
                else
                {
                    /* jeśli odebrano inny znak lub upłynął czas oczekiwania na 
                     * odpowiedź, to zakończenie pracy bootloadera */
                    break;
                }
            }
            
            /* jeśli komunikacja z komputerem urwała się podczas przesyłania 
             * strony, to bootloader jest uruchamiany ponownie, w przeciwnym 
             * przypadku zakończenie pracy bootloadera */
            if ( Is_Comm_Lost )
                Retries_Left = BOOT_WAIT * 10;
            else
                break;
        }
	}
	
    /* wyłączenie interfejsu UART (dzieje się to dopiero po zakończeniu 
     * wysyłania) i odczekanie do końca ewentualnej transmisji */
	UCSRB = 0;
    //_delay_us( UART_BYTE_TIME );
    
    /* skok do właściwego programu */
    #if defined( __AVR_HAVE_JMP_CALL__ )
        asm volatile ( 
            "jmp 0" "\n\t"
            : : );
    #elif defined( EIND )
        EIND = 0;
        asm volatile ( 
            "eijmp" "\n\t"
            : : "z" ( 0 ) );
    #else
        asm volatile ( 
            "ijmp" "\n\t"
            : : "z" ( 0 ) );
    #endif
    
    /* nieskończona pętla (konieczna, by kompilator nie wygenerował 
     * niepotrzebnego kodu) */
    while ( 1 )
        {}
}

static void UART_TX_Byte(
    const uint8_t Data )
{
	while ( !( UCSRA & 1 << UDRE ) )
        {}
    
	UDR = Data;
}

static void UART_TX_String_P(
    const char *String_Ptr )
{
	uint8_t Single_Char;
    
    #ifdef __AVR_HAVE_ELPM__
        while ( ( Single_Char = pgm_read_byte_far( String_Ptr++ ) ) )
    #else
        while ( ( Single_Char = pgm_read_byte_near( String_Ptr++ ) ) )
    #endif
            UART_TX_Byte( Single_Char );
}

static uint16_t _UART_RX_Wait( 
    uint16_t Timeout )
{
	do
    {
		if ( UCSRA & 1 << RXC )
            return ( uint16_t )UDR;
        
        _delay_us( UART_BYTE_TIME );
	} while ( --Timeout );
    
	return 0xffff;
}
