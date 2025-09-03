import { Link, useNavigate } from "react-router-dom";
import { InputComponent } from "./InputComponent";
import { useState } from "react";

import accountIconImage from "../files/images/accountIcon.png";
import shoppingCartIconImage from "../files/images/shoppingCartIcon.png";
import titleIcon from "../files/images/mgeCaterpillar.png";
import searchImage from "../files/images/searchImage.png";
import crossImage from "../files/images/cross.png";
import { ImageComponent } from "./ImageComponent";

export function Topbar({ auth }) {
  const maxPagesWrapperWidthForMobile = 1100;
  const navigator = useNavigate();

  function goToHome(path = "/") {
    navigator(path);
    window.location.reload();
    //If you go to the page you are on, react will not reload that page,
    //it will not change anything.
  }
  const [searchString, setSearchString] = useState("");
  const [needClear, setClear] = useState(false);
  const [isOpenWrapperForMobile, setOpenWrapperForMobile] = useState(false);
  const [menuButtonWasClicked, setMenuButtonWasClicked] = useState(false);

  function showForMobile() {
    const result = window.innerWidth <= maxPagesWrapperWidthForMobile;
    return result;
  }
  function accountLogo() {
    return (
      <li className="account-logo">
        <ImageComponent
          imageId={accountIconImage}
          linkTo={"/account"}
          imageClass="account-logo-image"
          linkClass="image-in-link account-image-link"
          getFromServer={false}
        />
      </li>
    );
  }
  function shoppingCartIcon() {
    return (
      <li className="shopping-cart-logo-image">
        <ImageComponent
          imageId={shoppingCartIconImage}
          linkTo={"/shopping-cart"}
          imageClass="shopping-cart-logo-image"
          linkClass="image-in-link shopping-cart-image-link"
          getFromServer={false}
        />
      </li>
    );
  }
  function pageRender(text, to) {
    return (
      <li className="topbar-page">
        <Link className="topbar-page-link" to={to}>
          {text}
        </Link>
      </li>
    );
  }

  function pages() {
    return (
      <>
        {auth.isAuth === true ? (
          <>
            {auth.isCustomer === false ? (
              <>
                {showForMobile() === true ? (
                  <>
                    {pageRender("Home", "/")}
                    {pageRender("Create", "/products/create")}
                    {pageRender("Account", "/account")}
                  </>
                ) : (
                  <>
                    {pageRender("Create", "/products/create")}
                    {accountLogo()}
                  </>
                )}
              </>
            ) : (
              <>
                {showForMobile() === true ? (
                  <>
                    {pageRender("Home", "/")}
                    {pageRender("Account", "/account")}
                    {pageRender("Cart", "/shopping-cart")}
                  </>
                ) : (
                  <>
                    {shoppingCartIcon()}
                    {accountLogo()}
                  </>
                )}
              </>
            )}
          </>
        ) : (
          <>{pageRender("Login", "/login")}</>
        )}
      </>
    );
  }
  return (
    <>
      <div className="topbar-content">
        <button className="topbar-title" onClick={() => goToHome()}>
          <img className="topbar-title-icon" src={titleIcon} />
          <h3 className="topbar-title-text">MarketPlace</h3>
        </button>
        <div className="global-search">
          <InputComponent
            id="globaSearchInput"
            type="text"
            beforeValidationFun={() => setClear(false)}
            afterValidationFun={(e) => setSearchString(e.target.value)}
            inputOtherProps={{
              className: "global-search-input",
              placeholder: "search...",
            }}
            needClear={needClear}
            valueAfterClear={""}
          />
          {searchString.length > 0 && (
            <button
              className="global-search-string-clean-button"
              onClick={() => {
                setClear(true);
                setSearchString("");
              }}
            >
              <img
                className="global-search-string-clean-button-image"
                src={crossImage}
              />
            </button>
          )}
          <button
            className="global-search-button"
            onClick={() => {
              if (searchString != "") {
                goToHome(`/?search=${searchString}`);
              }
            }}
          >
            find
          </button>
        </div>

        {auth.isAuth === true ? (
          showForMobile() ? (
            <button
              onClick={(e) => {
                const newValue = !isOpenWrapperForMobile;

                setMenuButtonWasClicked(true);
                if (newValue == false) e.target.blur();
                setOpenWrapperForMobile(newValue);
              }}
              className="menu-pages-icon"
            >
              ☰
            </button>
          ) : (
            <div className="topbar-pages-wrapper">
              <ul className="topbar-pages">{pages()}</ul>
            </div>
          )
        ) : (
          <ul className="topbar-pages mr-[10px]">{pages()}</ul>
        )}
      </div>

      {showForMobile() &&
        menuButtonWasClicked === true &&
        auth.isAuth === true && (
          <>
            {isOpenWrapperForMobile === true && (
              <div className="topbar-pages-for-mobile-line-on-bottom"></div>
            )}
            <ul
              className={
                menuButtonWasClicked === true
                  ? `
            topbar-pages-for-mobile ${
              isOpenWrapperForMobile ? "open" : "close"
            }`
                  : "topbar-pages-for-mobile"
              }
            >
              {pages()}
            </ul>
          </>
        )}
    </>
  );
}
