## 2. Runtime code generatie met Roslyn
Dit voorbeeld gebruikt Roslyn’s lage-niveau compileer-API (CSharpCompilation) in plaats van de scripting-API.
Hiermee:
- Genereer je écht een in-memory assembly,
- Kun je de bytecode laden en reflecteren,
- En heb je volledige controle over compilatie-opties, referenties en output.