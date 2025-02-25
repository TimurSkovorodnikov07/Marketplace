import { useState } from "react";
import { SellerInfoWriteComponent } from "../components/SellerInfoWriteComponent";
import { SellerProductCategoriesComponent } from "../components/SellerProductCategoriesComponent";
import { LogoutModalWindow } from "./LogoutModalWindow";
import { SearchInputs } from "./SearchInputs";
import {maxWidthForSmartphones} from "../configs/sizeNumbers"

export function SellerInfoComponent({
  sellerInfo,
  sellerId,
  isForOwner = false,
}) {
  const [search, setSearch] = useState("");
  const [priceNoMoreThenOrEqual, setPriceNoMoreThenOrEqual] = useState(0);

  console.log(sellerInfo);
  return (
    <>
      <div className="account-info-wrapper">
        <div className="account-info">
          <div className="mb-16">
            <SellerInfoWriteComponent
              sellerInfo={sellerInfo}
              isForOwner={isForOwner}
            />
          </div>

          <div className="account-inputs-wrapper">
            <SearchInputs
              setSearch={setSearch}
              setPriceNoMoreThenOrEqual={setPriceNoMoreThenOrEqual}
              priceNoMoreThenOrEqual={priceNoMoreThenOrEqual}
            />
          </div>
        </div>
        {window.innerWidth >= maxWidthForSmartphones && isForOwner === true && (
          <LogoutModalWindow />
        )}
      </div>
      <div className="account-partial-products-wrapper">
        <SellerProductCategoriesComponent
          sellerId={sellerId}
          priceNoMoreThenOrEqual={priceNoMoreThenOrEqual}
          search={search}
        />
      </div>

      {window.innerWidth < maxWidthForSmartphones && isForOwner === true && (
        <LogoutModalWindow />
      )}
    </>
  );
}
