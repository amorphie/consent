namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class PaymentRecordDto
{
    public string magic { get; set; }
    public string type { get; set; }
    public object headers { get; set; }
    public object messageSchemaId { get; set; }
    public object messageSchema { get; set; }
    public MessageDto message { get; set; }
}

public class MessageDto
{
    public DataDto data { get; set; }
    public object beforeData { get; set; }
    public HeadersDto headers { get; set; }
}

public class DataDto
{
    public string TRAN_DATE { get; set; } = string.Empty;
    public int TRAN_BRANCH_CODE { get; set; }
    public string REF_TYPE { get; set; }
    public int REF_NUM { get; set; }
    public int QUERY_NUMBER { get; set; }
    public string? MTRN { get; set; }
    public object? INNER_EFT_SEQUENCE_NUMBER { get; set; }
    public object? INCOMING_OPERATION_TIME { get; set; }
    public object? INCOMING_ARRIVAL_TIME { get; set; }
    public string? CREATION_TIME { get; set; }
    public int? RECEIVER_BANK { get; set; }
    public string? RECEIVER_BANK_NAME { get; set; }
    public int? RECEIVER_BRANCH { get; set; }
    public string? RECEIVER_BRANCH_NAME { get; set; }
    public int? RECEIVER_CITY_CODE { get; set; }
    public int? SENDER_BANK { get; set; }
    public int? SENDER_BRANCH { get; set; }
    public int? SENDER_CITY_CODE { get; set; }
    public string? MESSAGE_CODE { get; set; }
    public string? MESSAGE_MODULE { get; set; }
    public string? AMOUNT { get; set; }
    public string? PRIORITY { get; set; }
    public object? RECEIVED_FLAG { get; set; }
    public object? PARTIAL_TYPE { get; set; }
    public string? PART_NUMBER { get; set; }
    public object? IS_LAST_MESSAGE { get; set; }
    public object? SECURITY_CODE { get; set; }
    public string? QUANTITY { get; set; }
    public object? USER_REF { get; set; }
    public object? PARTIAL_PERMISSION { get; set; }
    public object? OUTER_DEPOSITORY { get; set; }
    public object? INNER_DEPOSITORY { get; set; }
    public int? ACCOUNT_BRANCH_CODE { get; set; }
    public int? ACCOUNT_BASIC_NUMBER { get; set; }
    public int? ACCOUNT_SUFFIX { get; set; }
    public object? ACCOUNT_NAME { get; set; }
    public string? TEXT_01 { get; set; }
    public string? TEXT_02 { get; set; }
    public string? TEXT_03 { get; set; }
    public object? TEXT_04 { get; set; }
    public string? TEXT_05 { get; set; }
    public string? TEXT_06 { get; set; }
    public object? TEXT_07 { get; set; }
    public object? TEXT_08 { get; set; }
    public object? TEXT_09 { get; set; }
    public object? TEXT_10 { get; set; }
    public object? TEXT_11 { get; set; }
    public object? TEXT_12 { get; set; }
    public object? TEXT_13 { get; set; }
    public object? TEXT_14 { get; set; }
    public object? TEXT_15 { get; set; }
    public object? TEXT_16 { get; set; }
    public string? AMOUNT_01 { get; set; }
    public string? AMOUNT_02 { get; set; }
    public string? AMOUNT_03 { get; set; }
    public string? AMOUNT_04 { get; set; }
    public string? AMOUNT_05 { get; set; }
    public string? AMOUNT_06 { get; set; }
    public string? AMOUNT_07 { get; set; }
    public string? AMOUNT_08 { get; set; }
    public string? AMOUNT_09 { get; set; }
    public string? AMOUNT_10 { get; set; }
    public string? AMOUNT_11 { get; set; }
    public string? AMOUNT_12 { get; set; }
    public string? AMOUNT_13 { get; set; }
    public string? AMOUNT_14 { get; set; }
    public string? AMOUNT_15 { get; set; }
    public string? AMOUNT_16 { get; set; }
    public string? AMOUNT_17 { get; set; }
    public string? AMOUNT_18 { get; set; }
    public string? AMOUNT_19 { get; set; }
    public object? DATE_01 { get; set; }
    public object? DATE_02 { get; set; }
    public object? DATE_03 { get; set; }
    public string? MESSAGE_GENERATOR_USER_CODE { get; set; }
    public int? OPERATION_BRANCH_CODE { get; set; }
    public int? ENTERANCE_ACCOUNTING_BRANCH_CODE { get; set; }
    public int? ENTERANCE_ACCOUNTING_REF { get; set; }
    public string? ENTERANCE_OPERATION_DATE { get; set; }
    public object? APPROVAL_USER_CODE { get; set; }
    public int? APPROVAL_ACCOUNTING_BRANCH_CODE { get; set; }
    public int? APPROVAL_ACCOUNTING_REF { get; set; }
    public object? APPROVAL_OPERATION_DATE { get; set; }
    public object? TRANSFER_FLAG { get; set; }
    public object? INTEGRATION_UPDATE_DATE { get; set; }
    public object? INTEGRATION_EXP1 { get; set; }
    public object? INTEGRATION_EXP2 { get; set; }
    public object? INTEGRATION_ERR_CODE { get; set; }
    public object? ERROR_MESSAGE { get; set; }
    public int ANNOUNCEMENT_BRANCH_CODE { get; set; }
    public object ANNOUNCEMENT_DATE { get; set; }
    public int ANNOUNCEMENT_NUMBER { get; set; }
    public int CANCELLATION_ACCOUNTING_BRANCH_CODE { get; set; }
    public int CANCELLATION_ACCOUNTING_REF { get; set; }
    public object? CANCELLATION_OPERATION_DATE { get; set; }
    public string? OPERATION_TYPE { get; set; }
    public string? EXPENSE_COMMISSION_AMOUNT { get; set; }
    public string? EXPENSE_COMMISSION_RATE { get; set; }
    public string? EXPENSE_COMMISSION_TAX_AMOUNT { get; set; }
    public string? EXPENSE_COMMISSION_TOTAL_AMOUNT { get; set; }
    public string? EXPENSE_EXCHANGE_TAX_AMOUNT { get; set; }
    public string? EXPENSE_EXPENSE_AMOUNT { get; set; }
    public string? EXPENSE_EXPENSE_RATE { get; set; }
    public string? EXPENSE_EXPENSE_TAX_AMOUNT { get; set; }
    public string? EXPENSE_EXPENSE_TOTAL_AMOUNT { get; set; }
    public string? EXPENSE_INCLUDED_FLAG { get; set; }
    public string? TOTAL_EXPENSE { get; set; }
    public object? EXPENSE_CASH_FLAG { get; set; }
    public int? EXPENSE_ACCOUNT_BRANCHCODE { get; set; }
    public int? EXPENSE_ACCOUNT_NUMBER { get; set; }
    public int? EXPENSE_ACCOUNT_SUFFIX { get; set; }
    public object? EXPENSE_TYPE { get; set; }
    public string? IBAN_FLAG { get; set; }
    public object? FIRST_RECEIVER_BRANCH { get; set; }
    public object? USER_MESSAGE { get; set; }
    public object? BATCH_REF_NUM { get; set; }
    public object? REFUSAL_TRAN_DATE { get; set; }
    public object? REFUSAL_TRAN_BRANCH_CODE { get; set; }
    public string? REFUSAL_REF_TYPE { get; set; }
    public object? REFUSAL_REF_NUM { get; set; }
    public object? REFUSAL_FIRST_RECORD_STATUS { get; set; }
    public string? LAST_UPDATING_USER_CODE { get; set; }
    public string? LAST_UPDATING_TRAN_CODE { get; set; }
    public string? LAST_UPDATING_CHANNEL_CODE { get; set; }
    public int? LAST_UPDATING_USER_ROLE { get; set; }
    public string LAST_UPDATE_DATE { get; set; }
    public string RECORD_STATUS { get; set; }
    public object? POOL_ACCOUNTING_BRANCH_CODE { get; set; }
    public object? POOL_ENTERANCE_ACCOUNTING_REF { get; set; }
    public object? POOL_ENTERANCE_OPERATION_DATE { get; set; }
    public int? ENTERANCE_USER_CASH_NUMBER { get; set; }
    public string? TRANSFER_REASON { get; set; }
    public object? SENDER_TAX_NO { get; set; }
    public object? RECEIVER_TAX_NO { get; set; }
    public object? PROVISION_NO { get; set; }
    public object? FINE_EXCEPTION { get; set; }
    public string? CONTAINS_DETAIL { get; set; }
    public object? IRN { get; set; }
    public string? IS_NEW_HABRGENL_FORMAT { get; set; }
    public string? PAS { get; set; }
    public string? FX { get; set; }
    public string? TRANSFER_SYSTEM { get; set; }
    public string? MESSAGE_TYPE { get; set; }
    public object? TEXT_17 { get; set; }
    public object? MESSAGE_SUB_TYPE { get; set; }
    public object? TEXT_18 { get; set; }
    public object? TEXT_19 { get; set; }
    public object? TEXT_20 { get; set; }
    public object? TEXT_21 { get; set; }
    public object? TEXT_22 { get; set; }
    public object? TEXT_23 { get; set; }
    public object? TEXT_24 { get; set; }
    public object? TEXT_25 { get; set; }
    public object? TEXT_26 { get; set; }
    public object? TEXT_27 { get; set; }
    public object? XML_01 { get; set; }
    public object? TEXT_28 { get; set; }
    public object? TEXT_29 { get; set; }
    public object? TEXT_30 { get; set; }
    public object? TEXT_31 { get; set; }
    public object? TEXT_32 { get; set; }
    public object? DATE_04 { get; set; }
    public object? TEXT_33 { get; set; }
    public object? TEXT_34 { get; set; }
    public object? TEXT_35 { get; set; }
    public object? TEXT_36 { get; set; }
}

public class HeadersDto
{
    public string? Operation { get; set; }
    public string? ChangeSequence { get; set; }
    public DateTime Timestamp { get; set; }
    // ... (other headers properties)
    public bool? TransactionLastEvent { get; set; }
}