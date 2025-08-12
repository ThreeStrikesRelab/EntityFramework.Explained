# Code of Conduct
> Voor alle studententeams/repo’s in dit vak. Dit document beschrijft **hoe** we samenwerken (workflow), **wat** we verwachten (kwaliteit), en **hoe** we elkaar helpen (review & communicatie).

## 1) Kernwaarden
* **Respect:** Feedback is op de code, niet op de persoon.
* **Transparantie:** Kleine, frequente commits met duidelijke boodschappen.
* **Samen leren:** Stel vragen vroeg, deel oplossingen en mislukkingen.
* **Eigenaarschap:** Jij bent verantwoordelijk voor jouw werk *en* voor het teamresultaat.

## 2) Repo‑model
* **Fork & PR‑model:** Studenten **forken** de hoofdrepo (`upstream`). Alle bijdragen gaan via **Pull Requests** vanuit je **fork** naar `main` van de upstream.
* **Branch‑strategie:**
  * `main`: altijd **groen** (builds slagen, tests groen).
  * Feature branches: `feature/<korte‑beschrijving>`
  * Bugfix branches: `fix/<issue‑nummer‑of‑korte‑beschrijving>`
  * Hotfix (kritieke prod‑bug): `hotfix/<beschrijving>`

## 3) Standaard workflow (stap‑voor‑stap)
1. **Fork** de hoofrepo op GitHub.
2. **Clone** je fork:
   ```bash
   git clone https://github.com/<jouw-account>/EntityFramework.Explained.git
   cd <repo>
   ```
3. **Koppel upstream** (éénmalig):
   ```bash
   git remote add upstream https://github.com/ThreeStrikesRelab/EntityFramework.Explained.git
   git fetch upstream
   ```
4. **Sync met upstream/main** (regelmatig):
   ```bash
   git checkout main
   git pull --ff-only upstream main
   git push origin main
   ```
5. **Maak een branch** voor je werk:
   ```bash
   git checkout -b feature/<korte-beschrijving>
   ```
6. **Codeer & test** lokaal. Voeg unit tests toe of actualiseer bestaande tests.
7. **Commit klein & vaak** (zie commit‑stijl hieronder).
8. **Push** je branch:
   ```bash
   git push -u origin feature/<korte-beschrijving>
   ```
9. **Open een Pull Request** naar `upstream/main`.
10. **Vraag review** (minstens 1 reviewer, bij voorkeur 2).
11. **Verwerk feedback** met extra commits, geen force pushes (overschrijf remote) tenzij afgesproken.
12. **Merge‑beleid:**
    * Squash‑merge (alle commits van branch => één commit) **na** groen CI en akkoord reviewers.
    * Upstream maintainer voert de merge uit.
13. **Opschonen:**
    ```bash
    git checkout main
    git pull --ff-only upstream main
    git branch -d feature/<korte-beschrijving>
    git push origin --delete feature/<korte-beschrijving>
    ```
## 4) Commit‑stijl (Conventional Commits, kort en informatief)
**Formaat:** `type(scope): korte beschrijving`
* **types:** `feat`, `fix`, `docs`, `refactor`, `build`, `ci`.
* **scope:**  map/onderdeel (`schema`, `behaviour`).
* **body:** *waarom* de wijziging nodig is, verwijs naar issue/PR indien aanwezig.
* **footer:** `BREAKING CHANGE:` of `Fixes #123`.

**Voorbeeld:**
```
feat(schema): initial explanation of ClassNullability
Using a simple model.
Fixes #42.
```

## 5) Pull Request‑richtlijnen
* **Klein is beter:** ≤ 400 regels diff is ideaal.
* **Checklist vóór je PR:**
  * [ ] Tests toegevoegd/aangepast
  * [ ] Alle tests groen lokaal
  * [ ] CI groen lokaal
  * [ ] Docs bijgewerkt. Check de source en lokale README.md. Deze laatste hoeft niet in commit te zitten.
  * [ ] Betekenisvolle titel & beschrijving (wat + waarom)
* **Link issues** met `Fixes #<nr>` of `Relates to #<nr>`.

## 6) Code review‑etiquette
* **Reviewer:**
  * Focus op correctheid, eenvoud, testbaarheid, naming.
  * Schrijf behulpzame, specifieke opmerkingen, toon voorbeelden.
  * Gebruik niveaus: `nit:` (klein), `suggestion:`, `question:`, `required:`.
* **Auteur:**
  * Antwoord op elke opmerking (ack/implement/weerleg met argumenten).
  * Vermijd defensiviteit, leer‑houding.
  * Maak follow‑up issues voor grotere verbeteringen buiten scope.

## 7) Test- & kwaliteitseisen
* **Unit tests:** bij elke nieuwe functionaliteit of bugfix.
* **CI:** PR’s worden pas gemerged als CI **groen** is.
* **Dekking:** richtlijn ≥ 70% op critical path; geen hard getal, wel gezond verstand.
* **Styles** volg projectstyle.

## 8) Branch‑bescherming & merge‑policy
* `main` is beschermd: minimaal 1 review, groene CI, geen direct pushes.
* **Squash‑merge** standaard => nette geschiedenis.
* **Rebase op main** indien je branch achterloopt:
  ```bash
  git fetch upstream
  git rebase upstream/main
  # los conflicten op
  git push -f origin feature/<korte-beschrijving>
  ```
  *Gebruik force‑push alleen op je **eigen** feature branch.*

## 9) Issues & planning
* **Issue types:** `feature`, `bug`, `doc`, `tech‑debt`.
* **Labels:** `good first issue`, `help wanted`, `priority:<hoog/midden/laag>`.
* **Definition of Ready (DoR):** duidelijk doel, acceptatiecriteria, kleine scope.
* **Definition of Done (DoD):** code + tests + docs + review + groen CI + merge.

## 10) Omgaan met conflicten
* **Technisch:** los mergeconflicten lokaal op, voeg tests toe om regressies te voorkomen.
* **Menselijk:** bespreek synchroon (voice/video) bij misverstanden, samenvatten in het PR.

## 11) Veiligheid & geheimen
* **No secrets** in code (API keys, connection strings). Gebruik **.gitignore**.
* **updates:** hou packages up‑to‑date, test na bump.

## 12) Grote bestanden & data
* **Geen binaries/exports** in git.
* **Sample data:** klein, synthetisch, privacy‑proof.

## 13) Documentatie
* **Readme up‑to‑date:** how‑to run, how‑to test. Geen probleem in dit geval normaal.

## 14) Communicatie
* **Kanaal:** Discord ([projectkanaal](https://discord.com/channels/707489155835232276/1404303820573380780)). Korte vragen => chat, complexe zaken => call + samenvatting in issue/PR.
* **Reactietijd:** best‑effort.
* **Besluitvorming:** vastleggen in issue/PR‑beschrijving.

## 15) AI‑gebruik (zoals ChatGPT)

* **Toegestaan**, maar: vermeld in PR‑beschrijving **waar** en **hoe** AI je geholpen heeft.
* Jij blijft verantwoordelijk voor correctheid, veiligheid, licentie & stijl.

## 16) Escalatie & support

* Blokkerende problemen > 15 minuten? **Escaleren**: noteer wat je probeerde, foutmeldingen, hypothesen; tag docent/coach in het issue.

### TL;DR
Fork → Sync → Branch → Code + Tests → Small Commits → Push → PR → Review → Groen CI → Squash‑Merge → Cleanup.

