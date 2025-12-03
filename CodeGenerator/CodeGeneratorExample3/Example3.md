## 3. Compile-time code generatie met Roslyn Source Analyzers
Source Generators draaien tijdens de compilatie zelf, binnen Roslyn.
Ze analyseren je code, lezen optioneel externe bestanden, en voegen automatisch nieuwe C#-bestanden toe aan de build.
Dit is ideaal om boilerplate te elimineren en patronen te automatiseren, zonder enige runtime-overhead.
In dit voorbeeld wordt tijdens compilatie een HelloWorld-class gegenereerd die informatie bevat over de aanwezige types 
in het project.
