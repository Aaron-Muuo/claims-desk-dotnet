# Claims Desk APi

ClaimsPortal.Api/
├── Controllers/         # API endpoints (equivalent to Laravel Controllers)
├── Data/                # DbContext, Migrations, and SQL execution helpers
├── DTOs/                # Request & Response data shapes (equivalent to Laravel FormRequests/Resources)
├── Models/              # Database entities (equivalent to Eloquent Models)
├── Services/            # Core business logic, workflows, and stored procedure calls
└── Program.cs           # Dependency injection, CORS, and middleware setup


## RUnning migrations

To create migration
```
dotnet ef migrations add InitialCreate
```

To apply migration
```
dotnet ef database update
```

To revert
```
dotnet ef migrations remove
```

To run the app

```
dotnet run
dotnet watch
```

To drop and run afresh
delete migration files in migrations folder then run:

```
dotnet ef database drop -f
dotnet ef migrations add InitialCreate
dotnet ef database update
```

OR run this sql

```
SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE Users;
TRUNCATE TABLE Roles;
TRUNCATE TABLE Organizations;

SET FOREIGN_KEY_CHECKS = 1;
```

### 5. How the Workflow Operates in Practice

| Stage | Action & Actor | Delegated Authority Rule |
| :--- | :--- | :--- |
| **1. Intake (FNOL)** | Customer files on portal OR staff files at front desk. | Status becomes `Submitted`. Channel reflects origin. |
| **2. Review** | Supervisor assigns to a `ClaimsOfficer`. | Status moves to `UnderReview`. |
| **3. Assessment** | Loss adjustor verifies documentation and sets `ApprovedAmount`. | If `ApprovedAmount <= Officer.ApprovalLimit` (e.g. KSh 100,000), officer approves directly. |
| **4. Escalation** | If `ApprovedAmount > KSh 100,000`, the officer cannot sign off. | System sets status to `PendingApproval` and reassigns to `UnderwritingManager` (up to KSh 500,000) or `FinanceDirector` (> KSh 500,000). |
| **5. Sign-off** | Authorized manager approves. | Status becomes `Approved`, recording `ApprovedByUserId`. |
| **6. Settlement** | Finance issues payment. | Status moves to `Settled`. |

## 1. Where AI Plugs into the Workflow Stages
AI automates document interpretation, policy verification, and risk triage, while the deterministic C# rules retain absolute control over financial sign-offs (DoA limits in KSh).

[1. Intake (FNOL)]          ---> AI Action: OCR & Extraction (Invoices, Police Abstracts)
         |
[2. UnderReview]            ---> AI Action: Policy Coverage Match & Missing Document Check
         |
[3. Assessment]             ---> AI Action: Repair Cost Benchmark & Fraud/Anomaly Scoring
         |
[4. Escalation / Sign-off]  ---> AI Action: Generates Executive Summaries for Managers
         |
[5. Settlement / Rejection] ---> AI Action: Drafts Statutory Discharge Vouchers / Rejection Letters


## Generate platform key

dotnet run -- --generate-platform-key