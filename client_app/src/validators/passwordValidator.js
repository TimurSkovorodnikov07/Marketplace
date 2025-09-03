import regexTest from "./regexTest";

export default function passwordValidator(email) {
  //at least 8-24 characters
  //at least 1 numeric character
  //at least 1 lowercase letter
  //at least 1 uppercase letter
  //at least 1 special character
  return regexTest(
    /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,24}$/,
    email
  );
}
