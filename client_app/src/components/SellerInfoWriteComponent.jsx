export function SellerInfoWriteComponent({ sellerInfo, isForOwner }) {
  return (
    <>
      <div className="account-info-element">
        <div className="account-info-label">Name: </div>
        <div className="account-info-element-text">{sellerInfo.name}</div>
      </div>
      {isForOwner === true && (
        <div className="account-info-element">
          <div className="account-info-label">Email: </div>
          <div className="account-info-element-text">{sellerInfo.email}</div>
        </div>
      )}
      <div className="account-info-element">
        <div className="account-info-label">Description: </div>{" "}
        <div className="account-info-element-text small-text">{sellerInfo.description}</div>
      </div>
      <div className="line-on-bottom"></div>
    </>
  );
}
