import regexTest from "./regexTest";

export const visaCardPattern = /^4[0-9]{12}(?:[0-9]{3})?$/;
export const masterCardPattern =
  /^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$/;

export function creditCardValidator(number, type, money) {
  return (
    ((regexTest(masterCardPattern, number) && type == "MasterCard") ||
      (regexTest(visaCardPattern, number) && type == "VisaCard")) &&
    creditCardMoneyValidator(money)
  );
}

export function creditCardNumberValidator(cardNum) {
  return (
    regexTest(masterCardPattern, cardNum) || regexTest(visaCardPattern, cardNum)
  );
}
export const maxMoneyNumber = 100000000;

export function creditCardMoneyValidator(money) {
  return money != undefined && money < 100000000 && money >= 0;
}
