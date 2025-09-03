import { useEffect, useState } from "react";
import { getCompanyInfo } from "../requests/deliveryCompanyRequests";
import { Link } from "react-router-dom";
import { ToolTip } from "./Tooltip";

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

  return (
    <>
      {info ? (
        <ToolTip linkTo={info.webSite} linkText={info.name}>
          <p>{info.description}</p>
          <p>{phoneNum}</p>
        </ToolTip>
      ) : (
        <></>
      )}
    </>
  );
}
