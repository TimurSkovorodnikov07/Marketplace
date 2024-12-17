import { useEffect, useState } from "react";
import { getCompanyInfo } from "../requests/deliveryCompanyRequests";
import { Link } from "react-router-dom";

export function DeliveryCompanyInfo({ deliveryCompanyId }) {
  const [info, setInfo] = useState(null);
  const [phoneNum, setPhoneNumber] = useState(undefined);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getCompanyInfo(deliveryCompanyId);

        const numWithoutSpaces = response.data.phoneNumber.trim();
        const resultNumber =
          numWithoutSpaces[0] == "+"
            ? numWithoutSpaces
            : `+ ${numWithoutSpaces}`;

        setInfo(response.data);
        setPhoneNumber(resultNumber);
      } catch (error) {
        console.error(error);
      }
    };
    asyncFun();
  }, []);

  //При касании на имя компании будет выползать окно которое уже более подробно пишет о компании(в широком описании компании, имя будет еще и ссылкой)
  //Это правда потом, тк мне нужно сделать это ебанное окно еще

  return (
    <>
      {info ? (
        <>
          <span className="tooltip">
            <Link to={info.webSite}>{info.name}</Link>
            <span className="tooltiptext">
              <p>{info.description}</p>
              <p>{phoneNum}</p>
            </span>
          </span>
        </>
      ) : (
        <></>
      )}
    </>
  );
  //фххахахах Я забыл что едлаю асинх запрос, потому стейт пока равен null
  //И вот блять думаю, ломаю голову, хули он орет о том что он null, реакт еще орет когда останавливаеться нахуй
  //Лады, моя ошибка
}
