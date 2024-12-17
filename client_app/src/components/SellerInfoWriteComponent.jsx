export function SellerInfoWriteComponent({ sellerInfo, isForOwner }) {
  return (
    <>
      <div className="account-info-element">
        <div className="account-info-label">Name: </div> {sellerInfo.name}
      </div>
      {isForOwner === true && (
        <div className="account-info-element">
          <div className="account-info-label">Email: </div> {sellerInfo.email}
        </div>
      )}
      <div className="account-info-element">
        <div className="account-info-label">Description: </div>{" "}
        {sellerInfo.description}
      </div>
      <div className="line-on-bottom"></div>
    </>
  );
}
