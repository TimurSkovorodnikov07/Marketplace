import { DeliveryCompanyInfo } from "./DeliveryCompanyInfo";
import { TagsComponent } from "./TagsComponent";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { AddToShoppingCartModalWindow } from "./AddToShoppingCartModalWindow";
import { SellerPartialInfo } from "./SellerPartialInfo";
import { UpdateAndRemoveButtons } from "./UpdateAndRemoveButtons";
import { GalleryComponent } from "./GalleryComponent";
import { Reviews } from "./Reviews";
import { Stars } from "./Stars";
import { BuyModalWindow } from "./BuyModalWindow";
import { NumberInputComponent } from "./NumberInputComponent";

export function ProductCategoryInfoComponent({
  category,
  isForOwner,
  haveInShoppingCart = false,
  defaultNumberOfPurchases = 1,
  isBought,
}) {
  const [numberOfPurchases, setNumberOfPurchases] = useState(
    defaultNumberOfPurchases
  );
  const [isAddToShoppingCart, setIsAddToShoppingCart] =
    useState(haveInShoppingCart);

  const [isRemoved, setIsRemoved] = useState(false);
  const navigate = useNavigate();
  function line() {
    return <div className="line-on-bottom"></div>;
  }
  function actionOnProduct() {
    return (
      <>
        {isForOwner === true ? (
          <>
            <div className="action-on-product-category">
              <UpdateAndRemoveButtons
                onUpdate={() => navigate(`/products/update/${category.id}`)}
                onRemove={() => {
                  setIsRemoved(true);
                  navigate("/");
                }}
                removedCategoryId={category.id}
              />
            </div>
          </>
        ) : (
          <>
            <div className="action-on-product-category">
              {category.quantity > 0 ? (
                <>
                  <AddToShoppingCartModalWindow
                    haveInShoppingCart={isAddToShoppingCart}
                    id={category.id}
                    numberOfPurchases={numberOfPurchases}
                    onAdd={() => setIsAddToShoppingCart(true)}
                    onClean={() => setIsAddToShoppingCart(false)}
                    className={"add-to-shopping-cart-button"}
                  />
                  {isAddToShoppingCart === false && (
                    <div className="product-category-buy-button">
                      <BuyModalWindow
                        list={[
                          {
                            id: category.id,
                            numberOfPurchases: numberOfPurchases,
                            name: category.name,
                          },
                        ]}
                      />
                    </div>
                  )}
                </>
              ) : (
                <></>
              )}
            </div>
          </>
        )}
      </>
    );
  }
  function getBaseInfo() {
    return (
      <>
        <h1 className="give-attention-big-text">{category.name}</h1>

        <div className="category-info">
          <div className="category-info-element">
            Price:{" "}
            <span className="give-attention-text">{category.price}$</span>
          </div>
        </div>

        <div className="category-info-element">
          <div className="category-estimation-element">
            <Stars
              estimation={category.totalEstimation}
              estimationCount={category.estimationCount}
            />
          </div>
        </div>
        <div className="tags category-info-element">
          <TagsComponent tags={category.tags} />
        </div>
        {isForOwner === false && (
          <>
            <div className="category-info-element">
              <SellerPartialInfo sellerId={category.ownerId} />
            </div>
          </>
        )}
        <div className="category-info-element">
          <div className="category-info-quantity-input-wrapper">
            {isForOwner == false && (
              <>
                {category.quantity > 1 ? (
                  <>
                    {line()}
                    <div className="give-attention-text">Quantity</div>
                    <NumberInputComponent
                      id="numberOfPurchasesInput"
                      value={numberOfPurchases}
                      setValue={setNumberOfPurchases}
                      min={1}
                      max={category.quantity}
                      inputOtherProps={{
                        min: 1,
                        max: category.quantity,
                        className: "number-input category-info-quantity-input",
                      }}
                    />
                  </>
                ) : (
                  <>{category.quantity === 0 && <div>Sorry, products is over...</div>}</>
                )}
              </>
            )}
          </div>
        </div>
        {line()}

        <div className="category-info-element">{actionOnProduct()}</div>
      </>
    );
  }
  return isRemoved === false ? (
    <>
      <div className="product-category-wrapper">
        <div className="main-product-category-info">
          <div className="gallery">
            <GalleryComponent categoryId={category.id} />
          </div>

          <div className="product-category-info">{getBaseInfo()}</div>
        </div>
      </div>

      <div className="other-product-category-info">
        <div className="give-attention-text">Delivery company: </div>
        <div className="category-info-element delivery-company-info">
          <DeliveryCompanyInfo deliveryCompanyId={category.deliveryCompanyId} />
        </div>
        {category.description != null && category.description?.length > 0 && (
          <>
            <div className="give-attention-text">Description: </div>
            <div className="category-info-element">{category.description}</div>
          </>
        )}
        <div className="reviews">
          <Reviews canWrite={isBought === true} categoryId={category.id} />
        </div>
      </div>
    </>
  ) : (
    <div>This product category removed</div>
  );
}
