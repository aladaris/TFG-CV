<CsoundSynthesizer>

    <CsOptions>

    ;RealTime audio

    -odac

    ; Menos verbose => Mejor rendimiento

    --m-amps=0

    --m-range=0

    --m-warnings=0

    ; MIDI

    -Ma -m0

    </CsOptions>

    <CsInstruments>
    sr = 44100

    ksmps = 32

    nchnls = 2

    0dbfs = 1.0
    
	#define KickSample  # 701 #
	#define SnareSample # 702 #
	#define HihatSample # 703 #
    
	#include "RAPUDO.txt"

    gi_Cos ftgen 0, 0, 2^10, 11, 1  ; Cosine Table



    ;; SUBSTRACTIVE 1 (TABLES)

    gi_DurTable1   ftgen 10, 0, 16, -2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1   ; Duracion de los pasos (1, 2 o 4 == Corchea, Negra o Blanca)

    gi_NotesTable1 ftgen 11, 0, 16, -2, 6.05, 6.08, 6.10, 6.08, 7.03, 7.01, 7.03, 7.05, 5.05, 5.08, 5.10, 5.08, 6.03, 6.01, 6.03, 6.05  ;; On the run

    gi_EnabledTable1 ftgen 12, 0, 16, -2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  ;  Pasos activos == 1, inactivos == 0

    ;; ADDITIVE 1 (TABLES)

    gi_DurTable2   ftgen 20, 0, 16, -2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2   ; Duracion de los pasos (1, 2 ó 4 == Corchea, Negra ó Blanca)

    gi_NotesTable2 ftgen 21, 0, 16, -2, 5.01, 5.01, 5.03, 5.01, 5.01, 5.01, 5.05, 5.01, 6.01, 6.01, 6.03, 6.01, 6.01, 6.01, 6.05, 6.01

    gi_EnabledTable2 ftgen 22, 0, 16, -2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1   ;  Pasos activos == 1, inactivos == 0

    ;; SAMPLER 1 (TABLES)
   
    gi_KickTable ftgen  30, 0, 16, -2,  1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1
    gi_SnareTable ftgen 31, 0, 16, -2,  0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0
    gi_HihatTable ftgen 32, 0, 16, -2,  0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0
	

  



    ;; SEQUENCER VARIABLES
    
    	; Track 1
    gk_Index1 init -1  ; NOTE: -1 en el init para que el primer trigger lo ponga en la primera posición
    gk_DurCount1 init 0 

    	; Track 2
    gk_Index2 init -1
    gk_DurCount2 init 0
    
    	; Track 3
    gk_Index3 init -1
    gk_DurCount3 init 0


    ;; ////////////////////

    ;; ///     MIDI     ///

    ;; ////////////////////

    massign 0,1

    instr 1

    	iinstrument = 30

    	kstatus, kchan, kdata1, kdata2 midiin

    	if (kstatus == 144) then

    		printk2 pchmidinn (kdata1) ;; DEBUG

    		event "i", iinstrument, 0, 1, pchmidinn (kdata1)

    	endif

    endin





    ;; ////////////////////

    ;; ///  GLOBAL LFO  ///

    ;; ////////////////////

    instr 5

    	ifreq = .05

    	imode = 1  ; 0 - Sine, 1 - Triangle, 2 - Square (bipolar), 3 - Square (unipolar), 4 - Sawtooth, 5 - Sawtooth (down)

    	gk_LFO lfo .5, ifreq, imode

    endin



    ;; Encendemos los intrumentos de apoyo

    	turnon 5 ;; LFO Global



    ;; ////////////////////

    ;; ///  SEQUENCER   ///

    ;; ////////////////////

    instr 10
    ; BPM
    kbpm invalue "bpm"
	
	  ; Volumen
	  gk_MainVol invalue "MainVol"
	  gk_Vol1 invalue "Vol1"
	  gk_Vol2 invalue "Vol2"
	  gk_Vol3 invalue "Vol3"
    
    ; Track Length
    gk_LengthTrack1 invalue "LengthTrack1"
    gk_LengthTrack2 invalue "LengthTrack2"
    gk_LengthTrack3 invalue "LengthTrack3"
    
    ; RitmicTrack Speed
    gk_RythmDur1 invalue "RythmDur1"
    
    ; Play / Pause
    gk_IsPlaying invalue "IsPlaying"  ;; 0: False (Paused), 1: True (Playing)
    
    ; Mode
    gk_SeqMode invalue "SeqMode"  ;; 0: Up, 1: Down
	
	
	
    	kidur = (kbpm * 2) / 60 ;; bpm * 2 para cuantificar como corcheas los valores 1 en las tablas de duracion
    	
    	kDurTrack1 init 0
    	kDurTrack2 init 0

    	ktimer phasor kidur
    	ktrg trigger ktimer, .99, 0

	

    	if ((ktrg == 1)  && (gk_IsPlaying == 1)) then

    		

    		;; TRACK 1

    		if (gk_DurCount1 == kDurTrack1) then
    			if (gk_SeqMode == 0) then
    				gk_Index1 = gk_Index1 + 1
	    			if gk_Index1 > gk_LengthTrack1 - 1 then
	    				gk_Index1 = 0
    				endif
    			elseif (gk_SeqMode == 1) then
    				gk_Index1 = gk_Index1 - 1
	    			if gk_Index1 < 0 then
	    				gk_Index1 = gk_LengthTrack1 - 1
    				endif
   			endif
          	outvalue  "Index_1", gk_Index1

    			kDurTrack1 table gk_Index1, gi_DurTable1, 0, 0, 1

    			knote 	  table gk_Index1, gi_NotesTable1, 0, 0, 1

			kactive   table gk_Index1, gi_EnabledTable1, 0, 0, 1

    			

          

    			kdur = (1/kidur)

    			kdur = kdur * kDurTrack1

    			;printk2 kdur ;; DEBUG

                if (kactive > 0) then

    			     event "i", 20, 0, kdur, knote, gk_Vol1 * gk_MainVol   ; FIRE: Substractive 1

                endif

          gk_DurCount1 = 0

    		endif

    		;; TRACK 2

    		if (gk_DurCount2 == kDurTrack2) then
    			if (gk_SeqMode == 0) then
    				gk_Index2 = gk_Index2 + 1
	    			if gk_Index2 > gk_LengthTrack2 - 1 then
	    				gk_Index2 = 0
    				endif
    			elseif (gk_SeqMode == 1) then
    				gk_Index2 = gk_Index2 - 1
	    			if gk_Index2 < 0 then
	    				gk_Index2 = gk_LengthTrack2 - 1
    				endif
   			endif

          outvalue  "Index_2", gk_Index2

    			kDurTrack2 table gk_Index2, gi_DurTable2, 0, 0, 1

    			knote 	  table gk_Index2, gi_NotesTable2, 0, 0, 1

            	kactive   table gk_Index2, gi_EnabledTable2, 0, 0, 1

    			kdur = (1/kidur)

    			kdur = kdur * kDurTrack2

                if (kactive > 0) then

    			    event "i", $String, 0, kdur, gk_Vol2 * gk_MainVol, knote

                endif

    			gk_DurCount2 = 0

    		endif

    		

    		;; TRACK 3

    		if (gk_DurCount3 >= gk_RythmDur1) then
    			if (gk_SeqMode == 0) then
    				gk_Index3 = gk_Index3 + 1
	    			if gk_Index3 > gk_LengthTrack3 - 1 then
	    				gk_Index3 = 0
    				endif
    			elseif (gk_SeqMode == 1) then
    				gk_Index3 = gk_Index3 - 1
	    			if gk_Index3 < 0 then
	    				gk_Index3 = gk_LengthTrack3 - 1
    				endif
   			endif

          outvalue  "Index_3", gk_Index3
                          
             kKick_Enabled table gk_Index3, gi_KickTable, 0, 0, 1
             kSnare_Enabled table gk_Index3, gi_SnareTable , 0, 0, 1
             kHihat_Enabled table gk_Index3, gi_HihatTable , 0, 0, 1
    			kdur = (1/kidur)
    			kdur = kdur * gk_RythmDur1

			if (kKick_Enabled == 1) then
				event "i", 40, 0, kdur, $KickSample, gk_Vol3 * gk_MainVol
			endif
			if (kSnare_Enabled == 1) then
				event "i", 40, 0, kdur, $SnareSample, gk_Vol3 * gk_MainVol
			endif
			if (kHihat_Enabled == 1) then
				event "i", 40, 0, kdur, $HihatSample, gk_Vol3 * gk_MainVol
			endif

    			gk_DurCount3 = 0

    		endif

    		

    		gk_DurCount1 = gk_DurCount1 + 1

    		gk_DurCount2 = gk_DurCount2 + 1

    		gk_DurCount3 = gk_DurCount3 + 1

    		

    	endif

    	;printk2 ktrg

    endin





    ;; ////////////////////

    ;; /// SUBSTRACTIVE ///

    ;; ////////////////////

    instr 20
    
    ivol = p5

    ; Envelope VCA

    	; TODO: Calcular los valores ADR con respecto a la duración de la nota (p4) para que sumen en total ese valor (p4).

    	iatt = p3 * .12   ; <P>

    	idec = p3 * .02    ; <P>

    	islev = .9   ; <P>

    	irel = p3 * .8   ; <P>

    	kaenv adsr iatt, idec, islev, irel  ; VCA's ADRS envelope
    	kaenv = kaenv / 2



    ;LFO1 - pitch

    	iltype = 4   ; LFO1 waveform  <P>

    	ildepth = 0  ; LFO1 amplitude <P>

    	ilfreq = 220  ; LFO1 frequency <P>
    	klfo lfo ildepth, ilfreq, iltype



    ; LFO2 - Filter

    	iltype2 = 0   ; LFO2 waveform    <P>

    	ildepth2 = 400  ; LFO2 amplitude

    	ilfreq2 = 5  ; LFO2 frequency   <P>

    	

    	klfo2 lfo ildepth2, ilfreq2, iltype2



    ; VCO 1

    	ifreq = cpspch(p4) ;         <S>

    imode = 10    ;               <S>  		;0 - Sawtooth, 2 - pwm sqr, 4 - pwm sawtooth/triangle/ramp, 6 - pulse, 10 - sqr (fast), 12 - triangle (fast)

    	iamp = ivol / 2  ; VCO1 amplitude <P>

    	kpwm = (0.99 - abs(gk_LFO)) / 2 ; VCO1 pwm       <P>

    	

      	a1 vco2 iamp * kaenv, ifreq + klfo, imode, kpwm, 0, .5
    		a1 limit a1, -1, 1  ; Prevent audio explosions



    ; VCO 2 (sub osc)

    	ifreq2 = ifreq / 2 ; VCO2 note value  <Divisor es P>

    imode2 = 4 ;                <P>        ;0 - Sawtooth, 2 - pwm sqr, 4 - pwm sawtooth/triangle/ramp, 6 - pulse, 10 - sqr (fast), 12 - triangle (fast)

    	iamp2 = ivol / 4  ; VCO2 amplitude <S>

    	kpwm2 = (0.01 + abs(gk_LFO)) / 2; VCO2 pwm       <P>  

    	

      	a2 vco2 iamp2 * kaenv, ifreq2 + klfo, imode2, kpwm2, 0, .5
      	a2 limit a2, -1, 1  ; Prevent audio explosions

      	avco sum a1, a2
      	avco limit avco, -1, 1  ; Prevent audio explosions



    ; NOISE

    	inoiseAmp = ivol / 66
    	 ;;  Range [0, 1]   <P>

    	inoiseBeta = -0.99   ;; Range [-1, 1]



    	a3 noise inoiseAmp * kaenv, inoiseBeta

    	anoise sum avco, a3
    	anoise limit anoise, -1, 1  ; Prevent audio explosions

    	

    ; VCF 1



    	icutoff = 360	 ; 				 <P>

    	ires = .8    ; 				 <P>

    	kfilterDepth = abs(gk_LFO); range [0, 1] <P>

    	iLFODepth = 0  ; range [0, 1] <P>

    	

    	af1 moogvcf2 anoise, icutoff + (klfo2 * iLFODepth), ires, 0dbfs

    	af1 sum ((1-kfilterDepth) * anoise), (af1 * kfilterDepth) ; Establecemos la cantidad de señal filtrada

	af1 limit af1, -1, 1  ; Prevent audio explosions

    	out af1, af1

    endin

    ;; ////////////////////

    ;; ///   SAMPLER    ///

    ;; ////////////////////

    ; TODO: Work In Progress

    gi_SampleKick ftgen  $KickSample, 0, 0, 1, "samples\kit1\kick.wav",  0, 0, 0

    gi_SampleHihat ftgen $HihatSample, 0, 0, 1, "samples\kit1\hihat.wav", 0, 0, 0

    gi_SampleSnare ftgen $SnareSample, 0, 0, 1, "samples\kit1\snare.wav", 0, 0, 0



    instr 40
		iamp = p5
	    	istable = p4
	    	icps = 1;p6
	    	ifn = 1;p7
		ibas = 1;p8
		a1 loscil iamp, icps, istable, ibas
		a1 limit a1, -1, 1  ; Prevent audio explosions
	    	out a1, a1
    endin

    </CsInstruments>   
    </CsoundSynthesizer>





