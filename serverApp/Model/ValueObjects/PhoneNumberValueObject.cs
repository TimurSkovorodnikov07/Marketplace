public class PhoneNumberValueObject
{
    public string Number { get; set; }

    public static PhoneNumberValueObject? Create(string phoneNumber)
    {
        var newPhoneNum = new PhoneNumberValueObject { Number = StringsExtensions.LeaveOnlyTheNumbers(phoneNumber) };
        return PhoneNumberValidator.IsValid(newPhoneNum) ? newPhoneNum : null;
    }
}