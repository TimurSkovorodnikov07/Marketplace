import regexTest from "./regexTest";

export function guidValidator(guid) {
  return guid && regexTest(/^[0-9A-Fa-f\-]{36}$/, guid);
}
