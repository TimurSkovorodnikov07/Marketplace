export function queryWithStringsCreate(path, typesAndValues) {
  let resultQuery = path;
  let curIndex = 0;

  typesAndValues.map((x) => {
    if (x.value != undefined && x.value != "") {
      resultQuery =
        resultQuery + queryString(x.type, x.value, curIndex === 0 ? "?" : "&");
      curIndex++;
    }
  });
  return resultQuery;
}
function queryString(type, value, symbol) {
  return value == undefined ? "" : `${symbol}${type}=${value}`;
}
