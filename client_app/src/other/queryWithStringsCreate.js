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
//Как я блять хуею с js
//From^%20%20%20%20%20%20%20%20%20%20 вместо норм числа, то же было и с to
//Я потом взял и разделил на 2 строки и их плюсанул, вуоля, блять, заработало сука нормально, ебануться
//100% из за того что я в ${} закидывал пустую строку, бляьт
