
namespace ClaimsDesk.API.Models.Enums;
public enum ClaimStatus
{
    Submitted = 1,          // Initial FNOL filing
    UnderReview = 2,        // Assigned to Claims Officer for document validation
    Assessment = 3,         // Loss adjustor / surveyor assessing damage
    PendingApproval = 4,    // Exceeds officer limit; escalated based on KSh threshold
    Approved = 5,           // Delegated authority signed off
    PaymentProcessing = 6,  // Queued for finance disbursement
    Settled = 7,            // Payment released, claim closed
    Rejected = 8            // Repudiated with justification
}

public enum SubmissionChannel
{
    CustomerPortal = 1,     // Submitted directly by policyholder
    InternalStaff = 2       // Filed by claims staff on behalf of customer
}

