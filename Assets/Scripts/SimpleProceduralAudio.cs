using UnityEngine;

public class SimpleProceduralAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isGenerating = false;
    private float frequency = 440f;
    private float gain = 0.5f;
    private double phase = 0;
    private double increment;
    private int sampleRate = 44100;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.volume = 1f;
    }

    public void PlayJumpSound()
    {
        Debug.Log("Reproduciendo sonido de salto simple");
        frequency = 600f;
        gain = 0.3f;
        PlayTone(0.2f);
    }

    public void PlayAttackSound()
    {
        Debug.Log("Reproduciendo sonido de ataque simple");
        frequency = 200f;
        gain = 0.4f;
        PlayTone(0.15f);
    }

    public void PlayCoinSound()
    {
        Debug.Log("Reproduciendo sonido de moneda estilo Mario");
        PlayMarioCoinSound();
    }

    public void PlaySwordSlashSound()
    {
        Debug.Log("Reproduciendo sonido de espada cortando el aire");
        // Sonido que simula una espada cortando el aire
        // Frecuencia que baja rápidamente (como el "whoosh" del viento)
        PlaySwordSlash();
    }

    public void PlayGemSound()
    {
        Debug.Log("Reproduciendo sonido de gema mágica");
        PlayMagicalGemSound();
    }

    public void PlayShieldSound()
    {
        Debug.Log("Reproduciendo sonido de escudo protector");
        PlayProtectiveShieldSound();
    }

    public void PlayChestOpenSound()
    {
        Debug.Log("Reproduciendo sonido de apertura de cofre");
        PlayTreasureChestSound();
    }

    public void PlayPortalSound()
    {
        Debug.Log("Reproduciendo sonido de portal mágico");
        PlayMagicalPortalSound();
    }

    public void PlayMaleVoiceSound()
    {
        PlayMaleVoiceBlip();
    }

    public void PlayStartGameSound()
    {
        Debug.Log("Reproduciendo sonido épico de iniciar juego");
        PlayEpicStartSound();
    }

    public void PlayResumeSound()
    {
        Debug.Log("Reproduciendo sonido suave de reanudar");
        PlaySoftResumeSound();
    }

    public void PlayQuitSound()
    {
        Debug.Log("Reproduciendo sonido final de salir");
        PlayFinalQuitSound();
    }

    public void PlayAgainSound()
    {
        Debug.Log("Reproduciendo sonido energético de reiniciar");
        PlayEnergeticAgainSound();
    }

    public void PlayDeathSound()
    {
        Debug.Log("Reproduciendo sonido dramático de muerte");
        PlayDramaticDeathSound();
    }

    public void PlayDamageSound()
    {
        Debug.Log("Reproduciendo sonido de daño/impacto");
        PlayImpactDamageSound();
    }

    void PlayTone(float duration)
    {
        if (isGenerating) return;

        // Crear AudioClip
        AudioClip clip = AudioClip.Create("ProceduralTone", (int)(sampleRate * duration), 1, sampleRate, false);

        // Generar datos de audio
        float[] samples = new float[(int)(sampleRate * duration)];
        increment = frequency * 2.0 * Mathf.PI / sampleRate;

        for (int i = 0; i < samples.Length; i++)
        {
            // Generar onda seno simple
            samples[i] = gain * Mathf.Sin((float)phase);
            phase += increment;

            // Aplicar envelope simple para evitar clicks
            float envelope = 1f;
            float fadeTime = 0.05f; // 50ms fade
            int fadeLength = (int)(sampleRate * fadeTime);

            if (i < fadeLength)
            {
                envelope = (float)i / fadeLength; // Fade in
            }
            else if (i > samples.Length - fadeLength)
            {
                envelope = (float)(samples.Length - i) / fadeLength; // Fade out
            }

            samples[i] *= envelope;
        }

        // Asignar datos al clip
        clip.SetData(samples, 0);

        // Reproducir
        audioSource.clip = clip;
        audioSource.Play();

        phase = 0; // Reset phase
    }

    void PlaySwordSlash()
    {
        float duration = 0.25f;
        AudioClip clip = AudioClip.Create("SwordSlash", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;

            // Frecuencia que baja de 400Hz a 100Hz (efecto whoosh)
            float currentFreq = 400f - (300f * time / duration);

            // Generar ruido filtrado que simula el aire
            float noise = UnityEngine.Random.Range(-1f, 1f);
            float sine = Mathf.Sin(2f * Mathf.PI * currentFreq * time);

            // Mezclar ruido y onda para el efecto de viento
            float sample = (0.7f * noise + 0.3f * sine) * 0.3f;

            // Envelope rápido: ataque súbito, decaimiento rápido
            float envelope = 1f;
            if (time < 0.02f) // Ataque súbito de 20ms
            {
                envelope = time / 0.02f;
            }
            else // Decaimiento suave
            {
                envelope = 1f - ((time - 0.02f) / (duration - 0.02f));
            }

            samples[i] = sample * envelope;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayDramaticDeathSound()
    {
        float duration = 1.5f;
        AudioClip clip = AudioClip.Create("DramaticDeath", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Sonido dramático de muerte: secuencia descendente trágica

            // FASE 1: Impacto inicial (0.0-0.3s)
            if (time < 0.3f)
            {
                // Acorde disonante inicial (impacto de muerte)
                float impact1 = Mathf.Sin(2f * Mathf.PI * 146f * time) * 0.4f; // D3
                float impact2 = Mathf.Sin(2f * Mathf.PI * 156f * time) * 0.3f; // D#3 (disonancia)
                float impact3 = Mathf.Sin(2f * Mathf.PI * 185f * time) * 0.2f; // F#3

                sample = impact1 + impact2 + impact3;

                // Envelope de impacto (ataque fuerte, decaimiento rápido)
                float impactEnv = Mathf.Exp(-6f * time);
                sample *= impactEnv;
            }

            // FASE 2: Descenso trágico (0.2-1.0s)
            if (time > 0.2f && time < 1.0f)
            {
                float tragicTime = time - 0.2f;

                // Secuencia descendente: C4 -> G3 -> F3 -> C3 (lamento)
                float[] tragicFreqs = { 261f, 196f, 175f, 131f };

                for (int n = 0; n < tragicFreqs.Length; n++)
                {
                    float noteStart = n * 0.2f;
                    if (tragicTime > noteStart && tragicTime < noteStart + 0.25f)
                    {
                        float noteTime = tragicTime - noteStart;
                        float note = Mathf.Sin(2f * Mathf.PI * tragicFreqs[n] * noteTime) * 0.3f;

                        // Envelope melancólico
                        float tragicEnv = Mathf.Exp(-2f * noteTime);
                        sample += note * tragicEnv;
                    }
                }
            }

            // FASE 3: Silencio final con eco (1.0-1.5s)
            if (time > 1.0f)
            {
                float echoTime = time - 1.0f;
                // Eco distante del último suspiro
                float echo = Mathf.Sin(2f * Mathf.PI * 131f * echoTime) * 0.15f * Mathf.Exp(-4f * echoTime);
                sample += echo;
            }

            // Envelope general
            float envelope = 1f;
            if (time < 0.02f)
            {
                envelope = time / 0.02f;
            }
            else if (time > 1.3f)
            {
                envelope = (1.5f - time) / 0.2f;
            }

            samples[i] = sample * envelope * 0.8f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayImpactDamageSound()
{
    float duration = 0.5f;
    AudioClip clip = AudioClip.Create("ManPainSound", (int)(sampleRate * duration), 1, sampleRate, false);
    
    float[] samples = new float[(int)(sampleRate * duration)];
    
    for (int i = 0; i < samples.Length; i++)
    {
        float time = (float)i / sampleRate;
        float sample = 0f;
        
        // QUEJIDO MASCULINO DE DOLOR - "AAHHH" o "UGHH"
        
        // Frecuencia base masculina (100-180Hz)
        float baseFreq = 130f + 30f * Mathf.Sin(8f * time); // Voz que fluctúa
        
        // Fundamental de la voz masculina
        float voiceFundamental = Mathf.Sin(2f * Mathf.PI * baseFreq * time) * 0.6f;
        
        // Primer armónico (da cuerpo a la voz)
        float harmonic1 = Mathf.Sin(2f * Mathf.PI * baseFreq * 2f * time) * 0.3f;
        
        // Segundo armónico (para realismo vocal)
        float harmonic2 = Mathf.Sin(2f * Mathf.PI * baseFreq * 3f * time) * 0.15f;
        
        // Sonido de quejido - modulación de dolor
        float painModulation = 1f + 0.4f * Mathf.Sin(2f * Mathf.PI * 6f * time);
        
        // Respiración entrecortada del dolor
        float breath = UnityEngine.Random.Range(-1f, 1f) * 0.1f * 
                      (0.5f + 0.5f * Mathf.Sin(2f * Mathf.PI * 15f * time));
        
        // Combinar componentes de voz masculina
        sample = (voiceFundamental + harmonic1 + harmonic2) * painModulation + breath;
        
        // Envelope del quejido (como si dijera "AAHHH!")
        float envelope = 1f;
        if (time < 0.1f)
        {
            envelope = time / 0.1f; // Ataque gradual "A..."
        }
        else if (time < 0.3f)
        {
            envelope = 1f; // Sostén "...AAA..."
        }
        else
        {
            envelope = (0.5f - time) / 0.2f; // Decaimiento "...hhh"
        }
        
        // Asegurar que no sea negativo
        envelope = Mathf.Max(0f, envelope);
        
        samples[i] = sample * envelope * 0.8f;
    }
    
    clip.SetData(samples, 0);
    audioSource.clip = clip;
    audioSource.Play();
}

    void PlayEpicStartSound()
    {
        float duration = 0.8f;
        AudioClip clip = AudioClip.Create("EpicStart", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Acorde épico ascendente: C4 -> E4 -> G4 -> C5 (Fanfare épico)
            if (time < 0.6f)
            {
                float[] heroicFreqs = { 261f, 329f, 392f, 523f };

                for (int n = 0; n < heroicFreqs.Length; n++)
                {
                    float noteStart = n * 0.15f;
                    if (time > noteStart && time < noteStart + 0.2f)
                    {
                        float noteTime = time - noteStart;
                        float note = Mathf.Sin(2f * Mathf.PI * heroicFreqs[n] * noteTime) * 0.4f;

                        // Armónico para sonido más potente
                        float harmonic = Mathf.Sin(2f * Mathf.PI * heroicFreqs[n] * 2f * noteTime) * 0.2f;

                        sample += note + harmonic;
                    }
                }
            }

            // Resonancia final épica
            if (time > 0.5f)
            {
                float finalTime = time - 0.5f;
                float finalNote = Mathf.Sin(2f * Mathf.PI * 523f * finalTime) * 0.5f * Mathf.Exp(-2f * finalTime);
                sample += finalNote;
            }

            samples[i] = sample * 0.7f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlaySoftResumeSound()
    {
        PlayTone(0.3f); // Usar el método existente con parámetros suaves
        frequency = 440f; // La4 - nota suave y agradable
        gain = 0.2f;
    }

    void PlayFinalQuitSound()
    {
        float duration = 0.6f;
        AudioClip clip = AudioClip.Create("FinalQuit", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Secuencia descendente final: G4 -> D4 -> C4 (despedida)
            float[] farewellFreqs = { 392f, 294f, 261f };

            for (int n = 0; n < farewellFreqs.Length; n++)
            {
                float noteStart = n * 0.2f;
                if (time > noteStart && time < noteStart + 0.25f)
                {
                    float noteTime = time - noteStart;
                    float note = Mathf.Sin(2f * Mathf.PI * farewellFreqs[n] * noteTime) * 0.3f;

                    // Envelope suave
                    float env = Mathf.Exp(-1.5f * noteTime);
                    sample += note * env;
                }
            }

            samples[i] = sample * 0.6f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayEnergeticAgainSound()
    {
        float duration = 0.5f;
        AudioClip clip = AudioClip.Create("EnergeticAgain", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Secuencia energética: E4 -> G4 -> B4 -> E5 (renovador)
            float[] energyFreqs = { 329f, 392f, 494f, 659f };

            for (int n = 0; n < energyFreqs.Length; n++)
            {
                float noteStart = n * 0.1f;
                if (time > noteStart && time < noteStart + 0.15f)
                {
                    float noteTime = time - noteStart;
                    float note = Mathf.Sin(2f * Mathf.PI * energyFreqs[n] * noteTime) * 0.35f;

                    // Efecto de energía
                    float energy = Mathf.Sin(2f * Mathf.PI * energyFreqs[n] * 1.5f * noteTime) * 0.15f;

                    sample += note + energy;
                }
            }

            samples[i] = sample * 0.6f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayMaleVoiceBlip()
    {
        float duration = 0.08f; // Muy corto para cada letra
        AudioClip clip = AudioClip.Create("MaleVoiceBlip", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        // Frecuencias típicas de voz masculina (100-300Hz fundamental)
        float baseFreq = UnityEngine.Random.Range(120f, 180f); // Fundamental masculina

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Componentes de voz masculina
            // Fundamental
            float fundamental = Mathf.Sin(2f * Mathf.PI * baseFreq * time) * 0.4f;

            // Primer armónico (octava)
            float harmonic1 = Mathf.Sin(2f * Mathf.PI * baseFreq * 2f * time) * 0.25f;

            // Segundo armónico (quinta)
            float harmonic2 = Mathf.Sin(2f * Mathf.PI * baseFreq * 3f * time) * 0.15f;

            // Tercer armónico (octava superior)
            float harmonic3 = Mathf.Sin(2f * Mathf.PI * baseFreq * 4f * time) * 0.1f;

            // Añadir un poco de ruido para realismo vocal
            float noise = UnityEngine.Random.Range(-1f, 1f) * 0.05f;

            // Combinar todos los componentes
            sample = fundamental + harmonic1 + harmonic2 + harmonic3 + noise;

            // Envelope muy rápido para simular consonantes/vocales cortas
            float envelope = 1f;
            if (time < 0.01f)
            {
                envelope = time / 0.01f; // Ataque rápido
            }
            else if (time > 0.05f)
            {
                envelope = (0.08f - time) / 0.03f; // Decaimiento rápido
            }

            // Aplicar modulación ligera para simular variación vocal
            float modulation = 1f + 0.1f * Mathf.Sin(2f * Mathf.PI * 8f * time);

            samples[i] = sample * envelope * modulation * 0.3f; // Volumen moderado
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayMagicalPortalSound()
    {
        float duration = 1.0f;
        AudioClip clip = AudioClip.Create("MagicalPortal", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Sonido de portal mágico: whoosh + energía dimensional + campanillas

            // FASE 1: Whoosh dimensional (efecto de succión/teletransporte)
            if (time < 0.6f)
            {
                // Frecuencia que baja rápidamente (efecto de succión)
                float whooshFreq = 300f - (250f * time / 0.6f); // 300Hz -> 50Hz
                float whoosh = Mathf.Sin(2f * Mathf.PI * whooshFreq * time) * 0.4f;

                // Ruido filtrado para simular energía dimensional
                float noise = UnityEngine.Random.Range(-1f, 1f) * 0.2f * (1f - time / 0.6f);

                sample += whoosh + noise;
            }

            // FASE 2: Campanillas mágicas (notas cristalinas)
            if (time > 0.2f && time < 0.9f)
            {
                float bellTime = time - 0.2f;

                // Secuencia de notas: E5 -> G5 -> B5 -> E6 (campanillas ascendentes)
                float[] bellFreqs = { 659f, 784f, 988f, 1319f };

                for (int n = 0; n < bellFreqs.Length; n++)
                {
                    float noteStart = n * 0.12f;
                    if (bellTime > noteStart && bellTime < noteStart + 0.25f)
                    {
                        float noteTime = bellTime - noteStart;

                        // Campanilla principal
                        float bell = Mathf.Sin(2f * Mathf.PI * bellFreqs[n] * noteTime) * 0.3f;

                        // Armónico para efecto cristalino
                        float harmonic = Mathf.Sin(2f * Mathf.PI * bellFreqs[n] * 3f * noteTime) * 0.1f;

                        // Envelope de campanilla (ataque rápido, decaimiento largo)
                        float bellEnv = Mathf.Exp(-3f * noteTime);

                        sample += (bell + harmonic) * bellEnv;
                    }
                }
            }

            // FASE 3: Resonancia dimensional final
            if (time > 0.7f)
            {
                float resonanceTime = time - 0.7f;

                // Acorde final misterioso (Am): A4, C5, E5
                float bass = Mathf.Sin(2f * Mathf.PI * 220f * resonanceTime) * 0.2f;
                float mid = Mathf.Sin(2f * Mathf.PI * 523f * resonanceTime) * 0.15f;
                float high = Mathf.Sin(2f * Mathf.PI * 659f * resonanceTime) * 0.1f;

                float chord = (bass + mid + high) * Mathf.Exp(-2f * resonanceTime);
                sample += chord;
            }

            // Efecto de "shimmer" durante todo el sonido
            float shimmer = Mathf.Sin(2f * Mathf.PI * 2000f * time) * 0.08f *
                           Mathf.Sin(2f * Mathf.PI * 12f * time) * (1f - time / duration);
            sample += shimmer;

            // Envelope general (fade in/out suave)
            float envelope = 1f;
            if (time < 0.05f)
            {
                envelope = time / 0.05f;
            }
            else if (time > 0.8f)
            {
                envelope = (1.0f - time) / 0.2f;
            }

            samples[i] = sample * envelope * 0.6f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayTreasureChestSound()
    {
        float duration = 1.2f;
        AudioClip clip = AudioClip.Create("TreasureChest", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Sonido de cofre del tesoro: creaking + magical reveal

            // FASE 1: Crujido de la madera/bisagras (0.0 - 0.4s)
            if (time < 0.4f)
            {
                // Sonido de crujido - frecuencias bajas irregulares
                float creak1 = Mathf.Sin(2f * Mathf.PI * (80f + 20f * Mathf.Sin(15f * time)) * time) * 0.3f;
                float creak2 = Mathf.Sin(2f * Mathf.PI * (120f + 30f * Mathf.Sin(8f * time)) * time) * 0.2f;

                // Ruido de fricción
                float friction = UnityEngine.Random.Range(-1f, 1f) * 0.1f * (1f - time / 0.4f);

                sample = creak1 + creak2 + friction;

                // Envelope que aumenta gradualmente (apertura lenta)
                float creakEnv = time / 0.4f;
                sample *= creakEnv;
            }

            // FASE 2: Momento de revelación mágica (0.3 - 1.2s)
            if (time > 0.3f)
            {
                float magicTime = time - 0.3f;

                // Arpeggio mágico ascendente: F4 -> A4 -> C5 -> F5
                float[] magicFreqs = { 349f, 440f, 523f, 698f };

                for (int n = 0; n < magicFreqs.Length; n++)
                {
                    float noteStart = n * 0.15f;
                    if (magicTime > noteStart && magicTime < noteStart + 0.3f)
                    {
                        float noteTime = magicTime - noteStart;
                        float magicNote = Mathf.Sin(2f * Mathf.PI * magicFreqs[n] * noteTime) * 0.4f;

                        // Añadir armónicos para efecto mágico
                        float harmonic = Mathf.Sin(2f * Mathf.PI * magicFreqs[n] * 2f * noteTime) * 0.15f;

                        sample += magicNote + harmonic;
                    }
                }

                // Efecto de "shimmer" (brillo mágico)
                if (magicTime < 0.8f)
                {
                    float shimmer = Mathf.Sin(2f * Mathf.PI * 1800f * magicTime) * 0.1f *
                                   Mathf.Sin(2f * Mathf.PI * 8f * magicTime);
                    sample += shimmer;
                }
            }

            // FASE 3: Resonancia final del tesoro (0.8 - 1.2s)
            if (time > 0.8f)
            {
                float resonanceTime = time - 0.8f;
                float resonance = Mathf.Sin(2f * Mathf.PI * 523f * resonanceTime) * 0.3f *
                                Mathf.Exp(-3f * resonanceTime);
                sample += resonance;
            }

            // Envelope general
            float envelope = 1f;
            if (time < 0.02f)
            {
                envelope = time / 0.02f; // Fade in suave
            }
            else if (time > 1.0f)
            {
                envelope = (1.2f - time) / 0.2f; // Fade out final
            }

            samples[i] = sample * envelope * 0.7f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayProtectiveShieldSound()
    {
        float duration = 0.6f;
        AudioClip clip = AudioClip.Create("ProtectiveShield", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Sonido de escudo: acorde potente que se expande
            // Notas del acorde C major: C4, E4, G4 (261Hz, 329Hz, 392Hz)

            if (time < 0.4f) // Acorde principal
            {
                // Nota fundamental C4
                float note1 = Mathf.Sin(2f * Mathf.PI * 261f * time) * 0.4f;
                // Tercera E4
                float note2 = Mathf.Sin(2f * Mathf.PI * 329f * time) * 0.3f;
                // Quinta G4
                float note3 = Mathf.Sin(2f * Mathf.PI * 392f * time) * 0.3f;

                sample = note1 + note2 + note3;

                // Añadir un poco de "metallic ring" para simular metal del escudo
                float metallic = Mathf.Sin(2f * Mathf.PI * 1200f * time) * 0.1f * Mathf.Exp(-5f * time);
                sample += metallic;
            }

            // Efecto de "whoosh" protector (como energía expandiéndose)
            if (time < 0.3f)
            {
                float whoosh = UnityEngine.Random.Range(-1f, 1f) * 0.15f * (1f - time / 0.3f);
                sample += whoosh;
            }

            // Envelope que simula la activación del escudo
            float envelope = 1f;
            if (time < 0.05f)
            {
                envelope = time / 0.05f; // Ataque rápido
            }
            else if (time < 0.15f)
            {
                envelope = 1f; // Sostener fuerte
            }
            else
            {
                // Decaimiento gradual con resonancia
                float fadeTime = time - 0.15f;
                envelope = Mathf.Exp(-2f * fadeTime) * (1f + 0.3f * Mathf.Sin(10f * fadeTime));
            }

            samples[i] = sample * envelope * 0.6f; // Volumen moderado
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayMagicalGemSound()
    {
        float duration = 0.8f;
        AudioClip clip = AudioClip.Create("MagicalGem", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Sonido de gema mágica: arpeggio ascendente con reverb
            // C6 -> E6 -> G6 -> C7 (1047Hz -> 1319Hz -> 1568Hz -> 2093Hz)

            if (time < 0.15f) // Primera nota C6
            {
                float freq = 1047f;
                sample = Mathf.Sin(2f * Mathf.PI * freq * time) * 0.3f;
            }
            else if (time < 0.3f) // Segunda nota E6
            {
                float freq = 1319f;
                float noteTime = time - 0.15f;
                sample = Mathf.Sin(2f * Mathf.PI * freq * noteTime) * 0.35f;
            }
            else if (time < 0.45f) // Tercera nota G6
            {
                float freq = 1568f;
                float noteTime = time - 0.3f;
                sample = Mathf.Sin(2f * Mathf.PI * freq * noteTime) * 0.4f;
            }
            else if (time < 0.65f) // Cuarta nota C7 (octava más alta)
            {
                float freq = 2093f;
                float noteTime = time - 0.45f;
                sample = Mathf.Sin(2f * Mathf.PI * freq * noteTime) * 0.45f;
            }

            // Agregar armónicos para sonido más mágico
            if (time < 0.65f)
            {
                float harmonic2 = Mathf.Sin(2f * Mathf.PI * (1047f + (time * 1000f)) * 2f * time) * 0.1f;
                float harmonic3 = Mathf.Sin(2f * Mathf.PI * (1047f + (time * 1000f)) * 3f * time) * 0.05f;
                sample += harmonic2 + harmonic3;
            }

            // Envelope general con cola larga (efecto mágico)
            float envelope = 1f;
            if (time < 0.02f)
            {
                envelope = time / 0.02f; // Ataque suave
            }
            else if (time > 0.65f)
            {
                // Cola larga que decae lentamente (efecto mágico)
                float fadeTime = time - 0.65f;
                envelope = Mathf.Exp(-3f * fadeTime); // Decaimiento exponencial
            }

            samples[i] = sample * envelope;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayMarioCoinSound()
    {
        float duration = 0.4f;
        AudioClip clip = AudioClip.Create("MarioCoin", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Sonido de moneda de Mario: B5 -> E6 (987Hz -> 1319Hz)
            if (time < 0.15f) // Primera nota (B5)
            {
                float freq = 987f;
                sample = Mathf.Sin(2f * Mathf.PI * freq * time) * 0.4f;

                // Envelope para la primera nota
                float env = 1f - (time / 0.15f) * 0.3f; // Decae ligeramente
                sample *= env;
            }
            else if (time < 0.4f) // Segunda nota (E6) - más alta y alegre
            {
                float freq = 1319f;
                float noteTime = time - 0.15f;
                sample = Mathf.Sin(2f * Mathf.PI * freq * noteTime) * 0.5f;

                // Envelope para la segunda nota
                float env = 1f - (noteTime / 0.25f) * 0.8f; // Decae más rápido
                sample *= env;
            }

            // Aplicar envelope general para evitar clicks
            float totalEnv = 1f;
            if (time < 0.01f)
            {
                totalEnv = time / 0.01f; // Fade in rápido
            }
            else if (time > 0.35f)
            {
                totalEnv = (0.4f - time) / 0.05f; // Fade out
            }

            samples[i] = sample * totalEnv;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySpikesSound()
    {
        Debug.Log("Reproduciendo sonido de púas punzantes");
        PlaySharpSpikesEffect();
    }
    
    public void PlayWaterSplashSound()
    {
        Debug.Log("Reproduciendo sonido de splash de agua");
        PlayDramaticSplashEffect();
    }
    
    public void PlayFireSound()
    {
        Debug.Log("Reproduciendo sonido de fuego");
        PlayBurningFireEffect();
    }
    
    public void PlayElectricSound()
    {
        Debug.Log("Reproduciendo sonido eléctrico");
        PlayElectricShockEffect();
    }
    
    public void PlayPoisonSound()
    {
        Debug.Log("Reproduciendo sonido de veneno");
        PlayToxicPoisonEffect();
    }

    public void PlayEnemyDeathSound()
    {
        Debug.Log("Reproduciendo sonido de muerte de enemigo");
        PlayEnemyDeathEffect();
    }

    void PlayEnemyDeathEffect()
    {
        // Sonido más corto y menos dramático que la muerte del jugador
        frequency = 200f;
        gain = 0.3f;
        PlayTone(0.8f);
    }
    
    // ===== IMPLEMENTACIONES DE EFECTOS =====

void PlaySharpSpikesEffect()
{
    // Grito agudo de dolor por púas
    float duration = 0.6f;
    AudioClip clip = AudioClip.Create("SpikesScream", (int)(sampleRate * duration), 1, sampleRate, false);
    
    float[] samples = new float[(int)(sampleRate * duration)];
    
    for (int i = 0; i < samples.Length; i++)
    {
        float time = (float)i / sampleRate;
        float sample = 0f;
        
        // Grito agudo que sube de frecuencia (dolor intenso)
        float screamFreq = 400f + (600f * time); // 400Hz -> 1000Hz
        float scream = Mathf.Sin(2f * Mathf.PI * screamFreq * time) * 0.5f;
        
        // Agregar distorsión de dolor
        float distortion = Mathf.Sin(2f * Mathf.PI * screamFreq * 3f * time) * 0.3f;
        
        // Ruido de sufrimiento
        float pain = UnityEngine.Random.Range(-1f, 1f) * 0.2f * (1f - time / duration);
        
        sample = scream + distortion + pain;
        
        // Envelope de grito (ataque fuerte, decaimiento)
        float envelope = Mathf.Exp(-2f * time) * (1f + 0.5f * Mathf.Sin(25f * time));
        
        samples[i] = sample * envelope * 0.8f;
    }
    
    clip.SetData(samples, 0);
    audioSource.clip = clip;
    audioSource.Play();
}

void PlayDramaticSplashEffect()
{
    // Grito ahogado en agua
    float duration = 1.0f;
    AudioClip clip = AudioClip.Create("DrowningScream", (int)(sampleRate * duration), 1, sampleRate, false);
    
    float[] samples = new float[(int)(sampleRate * duration)];
    
    for (int i = 0; i < samples.Length; i++)
    {
        float time = (float)i / sampleRate;
        float sample = 0f;
        
        // Grito que se ahoga progresivamente
        float drowningFreq = 300f - (150f * time / duration); // Se vuelve más grave
        float scream = Mathf.Sin(2f * Mathf.PI * drowningFreq * time) * 0.4f;
        
        // Efecto de burbujeo/ahogamiento
        float bubbles = Mathf.Sin(2f * Mathf.PI * 80f * time) * 0.2f * (time / duration);
        
        // Grito desesperado
        float desperation = Mathf.Sin(2f * Mathf.PI * 200f * time) * 0.3f * Mathf.Sin(10f * time);
        
        sample = scream + bubbles + desperation;
        
        // Envelope que simula ahogamiento
        float envelope = (1f - time / duration) * (1f + 0.3f * Mathf.Sin(15f * time));
        
        samples[i] = sample * envelope * 0.7f;
    }
    
    clip.SetData(samples, 0);
    audioSource.clip = clip;
    audioSource.Play();
}

void PlayBurningFireEffect()
{
    // Grito de dolor por quemadura
    float duration = 0.8f;
    AudioClip clip = AudioClip.Create("BurningScream", (int)(sampleRate * duration), 1, sampleRate, false);
    
    float[] samples = new float[(int)(sampleRate * duration)];
    
    for (int i = 0; i < samples.Length; i++)
    {
        float time = (float)i / sampleRate;
        float sample = 0f;
        
        // Grito intenso de quemadura
        float burnScream = Mathf.Sin(2f * Mathf.PI * 500f * time) * 0.6f;
        
        // Crepitar del fuego
        float crackle = UnityEngine.Random.Range(-1f, 1f) * 0.3f * Mathf.Sin(50f * time);
        
        // Armónicos de dolor
        float agony = Mathf.Sin(2f * Mathf.PI * 800f * time) * 0.2f * Mathf.Sin(12f * time);
        
        sample = burnScream + crackle + agony;
        
        // Envelope de dolor intenso
        float envelope = Mathf.Exp(-1.5f * time) * (1f + 0.4f * Mathf.Sin(20f * time));
        
        samples[i] = sample * envelope * 0.9f;
    }
    
    clip.SetData(samples, 0);
    audioSource.clip = clip;
    audioSource.Play();
}

void PlayElectricShockEffect()
{
    // Grito de electrocución
    float duration = 0.4f;
    AudioClip clip = AudioClip.Create("ElectricScream", (int)(sampleRate * duration), 1, sampleRate, false);
    
    float[] samples = new float[(int)(sampleRate * duration)];
    
    for (int i = 0; i < samples.Length; i++)
    {
        float time = (float)i / sampleRate;
        float sample = 0f;
        
        // Grito eléctrico intermitente
        float electricScream = Mathf.Sin(2f * Mathf.PI * 600f * time) * 0.7f;
        
        // Efecto de choque eléctrico
        float shock = UnityEngine.Random.Range(-1f, 1f) * 0.4f;
        
        // Modulación eléctrica
        float modulation = Mathf.Sin(2f * Mathf.PI * 60f * time); // 60Hz como electricidad
        
        sample = (electricScream + shock) * (0.5f + 0.5f * modulation);
        
        // Envelope de choque
        float envelope = 1f;
        if (time < 0.1f)
        {
            envelope = time / 0.1f;
        }
        else
        {
            envelope = Mathf.Exp(-8f * (time - 0.1f));
        }
        
        samples[i] = sample * envelope * 0.8f;
    }
    
    clip.SetData(samples, 0);
    audioSource.clip = clip;
    audioSource.Play();
}

    void PlayToxicPoisonEffect()
    {
        // Grito de envenenamiento
        float duration = 0.7f;
        AudioClip clip = AudioClip.Create("PoisonScream", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // Grito que se debilita por el veneno
            float poisonScream = Mathf.Sin(2f * Mathf.PI * 250f * time) * 0.5f;

            // Efecto tóxico burbujeante
            float toxic = Mathf.Sin(2f * Mathf.PI * 100f * time) * 0.2f * (time / duration);

            // Debilitamiento progresivo
            float weakness = Mathf.Sin(2f * Mathf.PI * 180f * time) * 0.3f * (1f - time / duration);

            sample = poisonScream + toxic + weakness;

            // Envelope de debilitamiento
            float envelope = (1f - 0.8f * time / duration) * (1f + 0.2f * Mathf.Sin(8f * time));

            samples[i] = sample * envelope * 0.6f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }
public void PlayHitEnemySound()
{
    Debug.Log("Reproduciendo sonido de golpe exitoso a enemigo");
    PlaySuccessfulHitEffect();
}

    void PlaySuccessfulHitEffect()
    {
        float duration = 0.8f; // Más largo
        AudioClip clip = AudioClip.Create("EnemyHit", (int)(sampleRate * duration), 1, sampleRate, false);

        float[] samples = new float[(int)(sampleRate * duration)];

        for (int i = 0; i < samples.Length; i++)
        {
            float time = (float)i / sampleRate;
            float sample = 0f;

            // SONIDO REALISTA DE GOLPE A PERSONA

            // FASE 1: Impacto inicial de la espada (0.0-0.2s)
            if (time < 0.2f)
            {
                // Sonido de metal cortando/impactando
                float metalCut = Mathf.Sin(2f * Mathf.PI * 300f * time) * 0.6f;

                // Sonido del impacto físico
                float bodyImpact = Mathf.Sin(2f * Mathf.PI * 150f * time) * 0.4f;

                // Ruido del golpe
                float hitNoise = UnityEngine.Random.Range(-1f, 1f) * 0.3f;

                sample = metalCut + bodyImpact + hitNoise;

                // Envelope de impacto fuerte
                float impactEnv = Mathf.Exp(-8f * time);
                sample *= impactEnv;
            }

            // FASE 2: Sonido del enemigo reaccionando (0.15-0.6s)
            if (time > 0.15f && time < 0.6f)
            {
                float reactTime = time - 0.15f;

                // Quejido/grito del enemigo golpeado
                float enemyPain = Mathf.Sin(2f * Mathf.PI * 180f * reactTime) * 0.4f;

                // Modulación de dolor
                float painMod = 1f + 0.3f * Mathf.Sin(2f * Mathf.PI * 8f * reactTime);

                sample += enemyPain * painMod;

                // Envelope del quejido
                float painEnv = Mathf.Exp(-3f * reactTime) * (1f + 0.2f * Mathf.Sin(12f * reactTime));
                sample *= painEnv;
            }

            // FASE 3: Eco del impacto y caída (0.4-0.8s)
            if (time > 0.4f)
            {
                float echoTime = time - 0.4f;

                // Reverberación del golpe
                float echo = Mathf.Sin(2f * Mathf.PI * 200f * echoTime) * 0.2f * Mathf.Exp(-4f * echoTime);

                // Sonido de movimiento/caída del enemigo
                float movement = UnityEngine.Random.Range(-1f, 1f) * 0.1f * (1f - echoTime / 0.4f);

                sample += echo + movement;
            }

            // Envelope general más natural
            float envelope = 1f;
            if (time < 0.03f)
            {
                envelope = time / 0.03f; // Ataque inicial
            }
            else if (time > 0.7f)
            {
                envelope = (0.8f - time) / 0.1f; // Fade out final
            }

            samples[i] = sample * envelope * 0.8f;
        }

        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }
// ===== SONIDOS ESPECÍFICOS CUANDO GOLPEAS ENEMIGOS =====

public void PlayEnemyHitSound(Enemy.EnemyType enemyType)
{
    switch (enemyType)
    {
        case Enemy.EnemyType.Human:
            Debug.Log("Golpeando humano/caballero");
            PlayHumanHitEffect();
            break;
        case Enemy.EnemyType.Undead:
            Debug.Log("Golpeando esqueleto/no-muerto");
            PlayUndeadHitEffect();
            break;
        case Enemy.EnemyType.Slime:
            Debug.Log("Golpeando slime");
            PlaySlimeHitEffect();
            break;
        case Enemy.EnemyType.Flying:
            Debug.Log("Golpeando criatura voladora");
            PlayFlyingHitEffect();
            break;
        case Enemy.EnemyType.Beast:
            Debug.Log("Golpeando bestia");
            PlayBeastHitEffect();
            break;
        case Enemy.EnemyType.Armor:
            Debug.Log("Golpeando armadura");
            PlayArmorHitEffect();
            break;
        case Enemy.EnemyType.Magic:
            Debug.Log("Golpeando criatura mágica");
            PlayMagicHitEffect();
            break;
        case Enemy.EnemyType.Giant:
            Debug.Log("Golpeando gigante");
            PlayGiantHitEffect();
            break;
    }
}

public void PlayEnemySpecificDeathSound(Enemy.EnemyType enemyType)
{
    // Sonidos de muerte específicos por tipo
    switch (enemyType)
    {
        case Enemy.EnemyType.Undead:
            PlaySkeletonDeathSound();
            break;
        case Enemy.EnemyType.Slime:
            PlaySlimeDeathSound();
            break;
        default:
            PlayEnemyDeathEffect(); // Sonido genérico
            break;
    }
}

// ===== EFECTOS DE GOLPEAR DIFERENTES TIPOS =====

void PlayHumanHitEffect()
{
    // Sonido metálico de golpear armadura/carne
    PlaySuccessfulHitEffect(); // Usa el que ya tienes
}

void PlayUndeadHitEffect()
{
    // Sonido de huesos quebrándose
    frequency = 400f;
    gain = 0.6f;
    PlayTone(0.5f);
}

void PlaySlimeHitEffect()
{
    // Sonido gelatinoso/baboso
    frequency = 200f;
    gain = 0.4f;
    PlayTone(0.6f);
}

void PlayFlyingHitEffect()
{
    // Sonido agudo de insecto/ave
    frequency = 800f;
    gain = 0.5f;
    PlayTone(0.3f);
}

void PlayBeastHitEffect()
{
    // Sonido de golpear animal/bestia
    frequency = 250f;
    gain = 0.7f;
    PlayTone(0.7f);
}

void PlayArmorHitEffect()
{
    // Sonido metálico muy fuerte
    frequency = 300f;
    gain = 0.8f;
    PlayTone(0.4f);
}

void PlayMagicHitEffect()
{
    // Sonido cristalino mágico
    frequency = 1000f;
    gain = 0.3f;
    PlayTone(0.4f);
}

void PlayGiantHitEffect()
{
    // Sonido masivo y profundo
    frequency = 100f;
    gain = 0.9f;
    PlayTone(0.8f);
}

void PlaySkeletonDeathSound()
{
    // Huesos cayendo/quebrándose
    frequency = 300f;
    gain = 0.5f;
    PlayTone(1.0f);
}

    void PlaySlimeDeathSound()
    {
        // Sonido de slime disolviéndose
        frequency = 150f;
        gain = 0.4f;
        PlayTone(1.2f);
    }
// ===== SONIDOS DE BOSS =====

public void PlayBossAttackSound(BossWeapon.BossType bossType)
{
    switch (bossType)
    {
        case BossWeapon.BossType.Knight:
            Debug.Log("Reproduciendo sonido de caballero golpeando");
            PlayKnightHitSound();
            break;
        case BossWeapon.BossType.Beast:
            Debug.Log("Reproduciendo sonido de bestia atacando");
            PlayBeastAttackSound();
            break;
        case BossWeapon.BossType.Undead:
            Debug.Log("Reproduciendo sonido siniestro de no-muerto");
            PlayUndeadAttackSound();
            break;
        case BossWeapon.BossType.Magic:
            Debug.Log("Reproduciendo sonido mágico");
            PlayMagicAttackSound();
            break;
        case BossWeapon.BossType.Giant:
            Debug.Log("Reproduciendo sonido masivo de gigante");
            PlayGiantAttackSound();
            break;
        case BossWeapon.BossType.Dragon:
            Debug.Log("Reproduciendo rugido épico de dragón");
            PlayDragonAttackSound();
            break;
    }
}

public void PlayBossSwingSound(BossWeapon.BossType bossType)
{
    // Sonido cuando el boss ataca al aire (más suave)
    switch (bossType)
    {
        case BossWeapon.BossType.Knight:
            PlayKnightSwingSound();
            break;
        // ... otros tipos
        default:
            PlaySwordSlashSound(); // Sonido genérico
            break;
    }
}

// ===== IMPLEMENTACIONES DE SONIDOS DE BOSS =====

void PlayKnightHitSound()
{
    // Sonido épico de caballero golpeando - metal pesado
    frequency = 250f;
    gain = 0.7f;
    PlayTone(0.6f);
}

void PlayKnightSwingSound()
{
    // Sonido de espada pesada cortando el aire
    frequency = 300f;
    gain = 0.5f;
    PlayTone(0.4f);
}

void PlayBeastAttackSound()
{
    // Rugido y garrazo de bestia
    frequency = 180f;
    gain = 0.8f;
    PlayTone(0.7f);
}

void PlayUndeadAttackSound()
{
    // Sonido siniestro de no-muerto
    frequency = 120f;
    gain = 0.6f;
    PlayTone(0.8f);
}

void PlayMagicAttackSound()
{
    // Sonido mágico cristalino
    frequency = 800f;
    gain = 0.5f;
    PlayTone(0.5f);
}

void PlayGiantAttackSound()
{
    // Sonido masivo y profundo
    frequency = 80f;
    gain = 0.9f;
    PlayTone(1.0f);
}

void PlayDragonAttackSound()
{
    // Rugido épico de dragón
    frequency = 200f;
    gain = 1.0f;
    PlayTone(1.2f);
}
}