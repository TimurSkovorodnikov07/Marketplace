import { useParams } from "react-router-dom";
import { SellerInfoComponent } from "../components/SellerInfoComponent";
import { useResponseCode } from "../hooks/customHooks";
import { getSellerInfo } from "../requests/sellerRequests";
import { useEffect, useState } from "react";
import { stringToBool } from "../other/converter";

export function SellerInfoPage() {
  const paramsForRoute = useParams();
  const sellerId = paramsForRoute.sellerId;
  const [sellerInfo, setSellerInfo] = useState(undefined);
  const [isForOwner, setForOwner] = useState(false);
  const [codeAndText, setCodeAndText] = useResponseCode();

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getSellerInfo(sellerId);

        if (response.status === 200) {
          const forOwnerFromHeaders = response.headers["x-get-for-owner"];

          setForOwner(stringToBool(forOwnerFromHeaders));
          setSellerInfo(response.data);
        }
        setCodeAndText({ code: response.status, text: response.statusText });
      } catch (e) {
        console.error(e);
        setCodeAndText({ code: e.status, text: e.statusText });
      }
    };
    asyncFun();
  }, []);

  function render() {
    switch (codeAndText.code) {
      case 200:
        return (
          <div className="account-info-container">
            <SellerInfoComponent
              sellerInfo={sellerInfo}
              sellerId={sellerId}
              isForOwner={isForOwner}
            />
          </div>
        );
      case 404:
        return <div>The Seller don't found</div>;
    }
  }
  return render();
}
