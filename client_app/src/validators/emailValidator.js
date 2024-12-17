import regexTest from "./regexTest";

export default function emailValidator(email) {
  return (
    regexTest(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/, email) && email.length < 45
  );
}
