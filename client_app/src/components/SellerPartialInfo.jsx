import { useEffect, useState } from "react";
import sellerIconImitation from "../files/images/accountIcon.png";
import { ImageComponent } from "./ImageComponent";
import { getSellerInfo } from "../requests/sellerRequests";

export function SellerPartialInfo({ sellerId }) {
  const [seller, setSeller] = useState(undefined);

  useEffect(() => {
    const asyncFun = async () => {
      const resposne = await getSellerInfo(sellerId);
      if (resposne.status === 200) {
        setSeller(resposne.data);
      }
    };
    asyncFun();
  }, [sellerId]);

  return seller === undefined ? (
    <></>
  ) : (
    <div className="seller-partial-info">
      <ImageComponent
        imageId={sellerIconImitation}
        linkTo={`/sellers/${sellerId}`}
        imageClass="seller-partial-icon"
        linkClass="seller-partial-icon-link"
        getFromServer={false}
      />
      <div>
        <div>{seller.name}</div>
      </div>
    </div>
  );
}
