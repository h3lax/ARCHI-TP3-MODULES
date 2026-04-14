# Sprint 2 — Modules, Ports & Adapters, CCP/CRP/REP

## Objectifs pedagogiques

- Organiser du code en modules (assemblies .NET) avec des frontieres explicites
- Appliquer les principes CCP, CRP et REP pour justifier le decoupage
- Comprendre les ports (driving / driven) et les adapters
- Maitriser l'impact de `public` vs `internal` sur le couplage

## Prerequisites

- .NET 8 SDK
- Un IDE (Rider, VS Code, Visual Studio)

## Rappel theorique

### Le module

Un module est une unite de deploiement et de changement. En .NET, c'est un projet/assembly.
Un module a :
- Un **interieur** (`internal`) : ce qui peut changer librement
- Un **exterieur** (`public`) : le contrat, la promesse aux consommateurs

Chaque type `public` est un point de couplage potentiel. Moins on en expose, plus on est libre.

### Ports & Adapters

- **Port driving** (primaire) : ce que le module **offre** (son API d'entree). Ex: `IBookingService`.
- **Port driven** (secondaire) : ce dont le module **a besoin**, defini comme une interface dans le module. Ex: `IReservationRepository`. L'implementation est ailleurs.
- Le port **appartient au module qui le definit**, pas a celui qui l'implemente.
- Un **adapter** implemente un port. C'est le cablage concret.

### CCP — Common Closure Principle

"Ce qui change ensemble vit ensemble." = SRP applique au module.
Si un changement de TVA impacte `TaxCalculator` et `InvoiceGenerator`, ils doivent etre dans le meme module.

### CRP — Common Reuse Principle

"Ne pas forcer a dependre de ce qu'on n'utilise pas." = ISP applique au module.
Si `Housekeeping` n'utilise que les dates de reservation, il ne devrait pas dependre d'un module qui contient aussi la facturation.

### REP — Reuse/Release Equivalence Principle

"L'unite de reutilisation est l'unite de release."
Un module doit etre coherent : tout ce qu'il contient a du sens ensemble.

### La tension entre les 3

- Trop de CCP (tout regrouper) → on force des dependances inutiles (viole CRP)
- Trop de CRP (tout separer) → un changement impacte 10 modules (viole CCP)
- Le bon module = l'equilibre entre les 3 forces

---

## Le code de depart

Ouvrez `Hotel.sln` a la racine du depot. Les modules vivent sous `Hotel/` : `Booking.Contracts` (contrats partages), `Booking`, `Billing`, `HouseKeeping`, `Common`, et `Runner` (composition root). Chaque assembly a son propre espace de noms (`Hotel.Booking.Contracts`, `Hotel.Billing`, etc.). Executez :

```bash
dotnet build
dotnet run --project Hotel/Runner/Hotel.Runner.csproj
```

### Constat

1. **Tout est `public`.** N'importe qui peut instancier n'importe quelle classe.
2. **Pas de frontiere.** `HousekeepingScheduler` peut acceder directement a `InvoiceGenerator`, alors qu'il n'en a pas besoin.
3. **Un seul namespace.** Impossible de savoir ce qui est un "contrat" vs un "detail d'implementation".

---

## Exercice 1 — Cartographie

Sur le code de depart :

1. **Listez** toutes les classes et interfaces publiques.
2. **Dessinez** le graphe de dependances : qui instancie/utilise qui ?
3. **Identifiez 3 clusters** de classes qui changent ensemble (indice : pensez aux acteurs du Sprint 1).

Repondez dans `ANSWERS.md`, section "Exercice 1".

---

## Exercice 2 — Decoupage en modules

Reorganisez le code en **plusieurs projets .NET** (assemblies distinctes).

### Consignes

1. Creez au minimum 4 projets : 3 modules metier + 1 module infrastructure + 1 runner.
2. Pour chaque module metier, creez un dossier `Contracts/` contenant **uniquement** les types publics (ports et DTOs).
3. Tout le reste doit etre marque `internal`.
4. Creez un `ServiceRegistration.cs` par module pour l'enregistrement DI.
5. Le `Hotel.Runner` est le **Composition Root** : c'est le seul projet qui reference tout le monde.
6. **Aucun module metier ne doit referencer un autre module metier directement**, sauf via ses `Contracts/`.
7. Les modules ne partagent PAS d'entite. Chaque module a sa propre vision des donnees.
8. Le output console doit rester **identique** au code de depart.

### Contraintes a verifier

- [ ] `dotnet build` compile sans erreur
- [ ] `dotnet run --project Hotel/Runner/Hotel.Runner.csproj` produit le meme output
- [ ] Aucune classe `internal` n'est accessible depuis un autre projet
- [ ] Les `.csproj` des modules metier ne se referencent pas entre eux (sauf via Contracts)

### Justification

Pour chaque module cree, repondez dans `ANSWERS.md` :
- Pourquoi ces classes sont-elles ensemble ? (CCP)
- Y a-t-il des classes que vous avez separees ? Pourquoi ? (CRP)
- Ce module pourrait-il etre reutilise independamment ? (REP)

---

## Exercice 3 — Test de la modification (note, 3 points)

### Scenario A — Changement de la politique de menage

L'hotel decide que le changement de draps aura lieu tous les **2 jours** au lieu de 3.

1. Appliquez ce changement sur votre solution.
2. Combien de **fichiers** avez-vous modifies ?
3. Combien de **modules** (projets) ont ete impactes ?

### Scenario B — Changement du taux de TVA

Le taux de TVA hebergement passe de 10% a **12%**.

1. Appliquez ce changement.
2. Combien de fichiers et modules impactes ?

### Scenario C — Nouveau canal de notification

On veut ajouter l'envoi de confirmation par **Push notification** en plus de l'email.

1. Que devez-vous creer / modifier ?
2. Les modules metier sont-ils impactes ?

### Analyse

Repondez dans `ANSWERS.md` :
- Quel principe garantit que le scenario A ne touche qu'un seul module ?
- Quel principe garantit que le scenario C n'impacte pas les modules metier ?

---

## Rappel

> Un module n'est pas un dossier. C'est une unite de deploiement
> avec un interieur protege et un exterieur contractuel.
> Le bon decoupage nait de la tension entre CCP, CRP et REP.
