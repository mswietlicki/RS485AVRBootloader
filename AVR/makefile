#----------------------------------------------------------------------------
# make all = Make software.
# make clean = Clean out built project files.
# make size = Display sizes of object files.
# make doc = Start doxygen to prepare documentation.
# make filename.s = Just compile filename.c into the assembler code only.
# make filename.i = Create a preprocessed source file.
# make program = Download the hex file to the device.
# make fuse = Download configuration bits to the device.
# make lock = Download lock bits to the device.
#----------------------------------------------------------------------------

# MCU name
MCU = atmega88

# MCU frequency (without UL suffix)
F_CPU = 8000000

# Bootloader options
# m128, m1284p  = 0x1FC00
# m32, m328 = 0x7E00
# m16, m168 = 0x3E00
# m8, m88 = 0x1E00
BLS_START = 0x1E00

# dla połączeń Bluetooth czas oczekiwania min 3 s
BOOT_WAIT = 1

BAUD_RATE = 9600

# MCU configuration fuse and lockbits (leave empty if not to be programmed or 
# to be programmed according to .fuse and .lock sections in ELF file)
LFUSE = 
HFUSE = 
EFUSE = 
LOCK = 

# Target file name (without extension)
TARGET = Bootloader

# Optimization level (can be 0, 1, 2, 3 or s)
OPT = s

# List C source files here (C dependencies are automatically generated)
SRC = main.c

# List Assembler source files here (make them always end in a capital .S!)
ASRC = 

# List any extra directories to look for include files here
EXTRAINCDIRS =

# Doxygen configuration file (leave empty if not available)
DOXYFILE = 

# C Standard level (can be c89, gnu89, c99 or gnu99)
CSTANDARD = -std=gnu99

# Output format (can be srec, ihex or binary)
FORMAT = ihex

# Debugging format
DEBUG = dwarf-2

# Programming hardware
AVRDUDE_PROGRAMMER = usbasp
AVRDUDE_PORT = lpt1

# Object files directory (don't leave it empty, put . instead!)
OBJDIR = .

# Place -D or -U options here for all sources
COMDEFS = -DBLS_START=$(BLS_START)
COMDEFS += -DBOOT_WAIT=$(BOOT_WAIT)
COMDEFS += -DBAUD=$(BAUD_RATE)
COMDEFS += -DMCU=$(MCU)
COMDEFS += -DXTAL=$(F_CPU) 

# Place -D or -U options here for C sources
CDEFS = 

# Place -D or -U options here for ASM sources
ADEFS = 


#---------------- Compiler Options C ----------------
#  -g*:          generate debugging information
#  -O*:          optimization level
#  -f...:        tuning, see GCC manual and avr-libc documentation
#  -Wall...:     warning level
#  -Wa,...:      tell GCC to pass this to the assembler.
#    -adhlns...: create assembler listing
CFLAGS = -g$(DEBUG)
CFLAGS += -DF_CPU=$(F_CPU)UL
CFLAGS += $(COMDEFS)
CFLAGS += $(CDEFS)
CFLAGS += -O$(OPT)
CFLAGS += -fgnu89-inline
CFLAGS += -funsigned-char
CFLAGS += -funsigned-bitfields
CFLAGS += -fpack-struct
CFLAGS += -fshort-enums
CFLAGS += -fno-tree-scev-cprop
CFLAGS += -fno-inline-small-functions
CFLAGS += -fno-split-wide-types
CFLAGS += -ffreestanding
CFLAGS += -mno-interrupts
#CFLAGS += -mshort-calls
CFLAGS += -pedantic
CFLAGS += -Wextra
CFLAGS += -Wno-sign-compare
CFLAGS += -Wstrict-prototypes
CFLAGS += -Wundef
#CFLAGS += -Wunreachable-code
CFLAGS += -Wa,-adhlns=$(<:%.c=$(OBJDIR)/%.lst)
CFLAGS += $(patsubst %,-I%,$(EXTRAINCDIRS))
CFLAGS += $(CSTANDARD)


#---------------- Assembler Options ----------------
#  -Wa,...:   tell GCC to pass this to the assembler.
#  -adhlns:   create listing
#  -gstabs:   have the assembler create line number information; note that
#             for use in COFF files, additional information about filenames
#             and function names needs to be present in the assembler source
#             files -- see avr-libc docs [FIXME: not yet described there]
#  -listing-cont-lines: Sets the maximum number of continuation lines of hex
#       dump that will be displayed for a given single line of source input.
ASFLAGS = -gstabs
ASFLAGS += -DF_CPU=$(F_CPU)
ASFLAGS += $(COMDEFS)
ASFLAGS += $(ADEFS)
ASFLAGS += -Wa,-adhlns=$(<:%.S=$(OBJDIR)/%.lst)
ASFLAGS += -listing-cont-lines=100


#---------------- Library Options ----------------
# List any extra directories to look for libraries here.
#     Each directory must be seperated by a space.
#     Use forward slashes for directory separators.
#     For a directory that has spaces, enclose it in quotes.
EXTRALIBDIRS =


#---------------- Linker Options ----------------
#  -Wl,...:     tell GCC to pass this to linker.
#    -Map:      create map file
#    --cref:    add cross reference to  map file
LDFLAGS = -Wl,-Map=$(TARGET).map,--cref
#LDFLAGS += -Wl,--relax
LDFLAGS += -nostartfiles
LDFLAGS += -Wl,--section-start=.text=$(BLS_START)
LDFLAGS += $(patsubst %,-L%,$(EXTRALIBDIRS))
#LDFLAGS += -T linker_script.x


#---------------- Programming Options (avrdude) ----------------
AVRDUDE_FLAGS = -p $(MCU) -P $(AVRDUDE_PORT) -c $(AVRDUDE_PROGRAMMER)


#============================================================================


# Define programs and commands.
SHELL = sh
CC = avr-gcc
OBJCOPY = avr-objcopy
OBJDUMP = avr-objdump
SIZE = avr-size
AR = avr-ar rcs
NM = avr-nm
AVRDUDE = avrdude
REMOVE = rm -f
REMOVEDIR = rm -rf
COPY = cp
WINSHELL = cmd
DOXYGEN = doxygen


# Define all object files.
OBJ = $(SRC:%.c=$(OBJDIR)/%.o) $(ASRC:%.S=$(OBJDIR)/%.o)

# Define all listing files.
LST = $(SRC:%.c=$(OBJDIR)/%.lst) $(ASRC:%.S=$(OBJDIR)/%.lst)


# Compiler flags to generate dependency files.
GENDEPFLAGS = -MMD -MP -MF .dep/$(@F).d


# Combine all necessary flags and optional flags.
# Add target processor to flags.
ALL_CFLAGS = -mmcu=$(MCU) -I. $(CFLAGS) $(GENDEPFLAGS)
ALL_ASFLAGS = -mmcu=$(MCU) -I. -x assembler-with-cpp $(ASFLAGS)


#============================================================================


# Default target.
all: begin sizebefore build sizeafter end

# Change the build target to build a HEX file or a library.
build: elf hex lss sym
#build: lib


burn: fuse program lock

elf: $(TARGET).elf
hex: $(TARGET).hex
lss: $(TARGET).lss
sym: $(TARGET).sym
LIBNAME=lib$(TARGET).a
lib: $(LIBNAME)


# Eye candy.
begin:
	@echo
	@echo -------- begin --------

end:
	@echo --------  end  --------
	@echo


# Display size of file.
HEXSIZE = $(SIZE) --target=$(FORMAT) $(TARGET).hex
ELFSIZE = $(SIZE) --mcu=$(MCU) --format=avr $(TARGET).elf

sizebefore:
	@if test -f $(TARGET).elf; then echo; echo Size before:; $(ELFSIZE); \
	2>/dev/null; fi

sizeafter:
	@if test -f $(TARGET).elf; then echo; echo Size after:; $(ELFSIZE); \
	2>/dev/null; fi


# Program the device.
program: $(TARGET).hex
	@echo
	@echo Programming Flash memory:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U flash:w:$(TARGET).hex


# Program fusebits.
fuse: $(TARGET).elf
	@echo
ifdef LFUSE
	@echo Programming low fusebits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U lfuse:w:$(LFUSE):m
else
	@echo Creating load file for low fusebits:
	-$(OBJCOPY) -j .fuse --change-section-lma .fuse=0 --no-change-warnings \
	-i 3 -b 0 -O $(FORMAT) $< $(TARGET).lfuse || exit 0
	@echo
	@echo Programming low fusebits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U lfuse:w:$(TARGET).lfuse    
endif
	@echo
ifdef HFUSE
	@echo Programming high fusebits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U hfuse:w:$(HFUSE):m
else
	@echo Creating load file for high fusebits:
	-$(OBJCOPY) -j .fuse --change-section-lma .fuse=0 --no-change-warnings \
	-i 3 -b 1 -O $(FORMAT) $< $(TARGET).hfuse || exit 0
	@echo
	@echo Programming high fusebits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U hfuse:w:$(TARGET).hfuse    
endif
	@echo
ifdef EFUSE
	@echo Programming extended fusebits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U efuse:w:$(EFUSE):m
else
	@echo Creating load file for extended fusebits:
	-$(OBJCOPY) -j .fuse --change-section-lma .fuse=0 --no-change-warnings \
	-i 3 -b 2 -O $(FORMAT) $< $(TARGET).efuse || exit 0
	@echo
	@echo Programming extended fusebits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U efuse:w:$(TARGET).efuse    
endif


# Program lockbits.
lock: $(TARGET).elf
	@echo
ifdef LOCK
	@echo Programming lockbits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U lock:w:$(LOCK):m
else
	@echo Creating load file for lockbits:
	-$(OBJCOPY) -j .lock --change-section-lma .lock=0 --no-change-warnings \
	-O $(FORMAT) $< $(TARGET).lock || exit 0
	@echo
	@echo Programming lockbits:
	$(AVRDUDE) $(AVRDUDE_FLAGS) -U lock:w:$(TARGET).lock
endif


# Generate documentation with Doxygen.
doc:
	@if test -f $(DOXYFILE); echo; then $(DOXYGEN) $(DOXYFILE); fi


# Display sizes of the object files.
size: $(OBJ)
	@echo
	@echo Sizes of the object files "(Flash = .text + .data, RAM = .data + .bss)":
	$(SIZE) -B -d -t --common $(OBJ)


# Create final output file (.hex) from ELF output file.
%.hex: %.elf
	@echo
	@echo Creating load file for Flash: $@
	$(OBJCOPY) -O $(FORMAT) -R .eeprom -R .fuse -R .lock $< $@

# Create extended listing file from ELF output file.
%.lss: %.elf
	@echo
	@echo Creating Extended Listing: $@
	$(OBJDUMP) -h -S -z $< > $@

# Create a symbol table from ELF output file.
%.sym: %.elf
	@echo
	@echo Creating Symbol Table: $@
	$(NM) -n $< > $@


# Create library from object files.
.SECONDARY : $(TARGET).a
.PRECIOUS : $(OBJ)
%.a: $(OBJ)
	@echo
	@echo Creating library: $@
	$(AR) $@ $(OBJ)


# Link: create ELF output file from object files.
.SECONDARY : $(TARGET).elf
.PRECIOUS : $(OBJ)
%.elf: $(OBJ)
	@echo
	@echo Linking: $@
	$(CC) $(ALL_CFLAGS) $^ --output $@ $(LDFLAGS)


# Compile: create object files from C source files.
$(OBJDIR)/%.o : %.c
	@echo
	@echo Compiling C: $<
	$(CC) -c $(ALL_CFLAGS) $< -o $@


# Compile: create assembler files from C source files.
%.s : %.c
	$(CC) -S $(ALL_CFLAGS) $< -o $@


# Assemble: create object files from assembler source files.
$(OBJDIR)/%.o : %.S
	@echo
	@echo Assembling: $<
	$(CC) -c $(ALL_ASFLAGS) $< -o $@


# Create preprocessed source for use in sending a bug report.
%.i : %.c
	$(CC) -E -mmcu=$(MCU) -I. $(CFLAGS) $< -o $@


# Target: clean project.
clean: begin clean_list end

clean_list :
	@echo
	@echo Cleaning project:
	$(REMOVE) $(TARGET).hex
	$(REMOVE) $(TARGET).lfuse
	$(REMOVE) $(TARGET).hfuse
	$(REMOVE) $(TARGET).efuse
	$(REMOVE) $(TARGET).lock
	$(REMOVE) $(TARGET).elf
	$(REMOVE) $(TARGET).map
	$(REMOVE) $(TARGET).sym
	$(REMOVE) $(TARGET).lss
	$(REMOVE) $(SRC:%.c=$(OBJDIR)/%.o)
	$(REMOVE) $(SRC:%.c=$(OBJDIR)/%.lst)
	$(REMOVE) $(ASRC:%.S=$(OBJDIR)/%.o)
	$(REMOVE) $(ASRC:%.S=$(OBJDIR)/%.lst)
	$(REMOVE) $(SRC:.c=.s)
	$(REMOVE) $(SRC:.c=.d)
	$(REMOVE) $(SRC:.c=.i)
	$(REMOVEDIR) .dep


# Create object files directory
$(shell mkdir $(OBJDIR) 2>/dev/null)


# Include the dependency files.
-include $(shell mkdir .dep 2>/dev/null) $(wildcard .dep/*)


# Listing of phony targets.
.PHONY : all begin finish end sizebefore sizeafter \
build elf hex lss sym  \
clean clean_list program fuse lock doc
