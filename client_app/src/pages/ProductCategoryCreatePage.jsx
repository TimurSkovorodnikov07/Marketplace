import { useEffect, useState } from "react";
import {
  categoryNameIsFree,
  productCategoryCreate,
} from "../requests/productCategoriesRequests";
import { useResponseCode } from "../hooks/customHooks";
import { useSelector } from "react-redux";
import { stringToBool } from "../other/converter";
import { InputComponent } from "../components/InputComponent";
import {
  descriptionInvalidText,
  nameInvalidText,
  priceInvalidText,
  quantityInvalidText,
} from "../configs/TextsDuringInvalidity";
import categoryNameValidator from "../validators/categoryNameValidator";
import {
  descriptionValidator,
  descriptionWithEmptyValidator,
} from "../validators/descriptionValidator";
import { useNavigate } from "react-router-dom";
import { greaterThanZeroValidator } from "../validators/greaterThanZeroValidator";
import { AddTagsComponent } from "../components/AddTagsComponent";
import { SelectDeliveryCompanyComponent } from "../components/SelectDeliveryCompanyComponent";
import { ChooseImagesComponent } from "../components/ChooseImagesComponent";
import { guidValidator } from "../validators/guidValidator";
import { TextAreaComponent } from "../components/TextAreaComponent";

export async function nameIsFreeCheck(name) {
  try {
    const nameIsFreeResponse = await categoryNameIsFree(name);
    return nameIsFreeResponse.status === 200;
  } catch (error) {
    console.error(error);
  }
  return false;
}

export function ProductCategoryCreatePage() {
  const nameIsOccupiedText = "The Name is occupied";

  const [codeAndText, setCodeAndText] = useResponseCode();
  const isCustomer = useSelector((state) => state.auth.isCustomer);
  const nagivate = useNavigate();
  const [isAllValid, setIsAllValid] = useState(true);

  const [name, setName] = useState(undefined);
  const [nameIsFree, setNameIsFree] = useState(true);

  const [description, setDescription] = useState(undefined);
  const [tags, setTags] = useState([]);
  const [price, setPrice] = useState(undefined);
  const [quantity, setQuantity] = useState(undefined);
  const [deliveryCompany, setDeliveryCompany] = useState(undefined);
  const [images, setImages] = useState([]);

  async function sent() {
    try {
      const response = await productCategoryCreate(
        name,
        description,
        tags,
        price,
        quantity,
        deliveryCompany.id,
        images
      );

      setCodeAndText({
        code: response.status,
        text: response.statusText,
      });

      if (response.status === 200) {
        const newCategoryId = response.data;
        nagivate(`/products/${newCategoryId}`);
      }
    } catch (error) {
      if (error.response.status === 409) setNameIsFree(false);
      setCodeAndText({
        code: error.response.status,
        text: error.response.data,
      });
    }
  }

  function render() {
    switch (codeAndText) {
      case 200:
        return <></>;
      case 403:
        return <div>You not seller</div>;
      default:
        return (
          <div className="product-category-create-or-update-page-wrapper">
            <h1 className="pb-10">Create a new product category</h1>
            <div className="product-category-create-or-update-wrapper">
              <h2>Name:</h2>
              <InputComponent
                id="name"
                type="text"
                invalidText={nameInvalidText}
                validatorFun={(value) => {
                  return (
                    categoryNameValidator(value) === true
                  );
                }}
                afterValidationFun={(e) => {
                  setNameIsFree(true);
                  setName(e.target.value);
                }}
                invalidatedFun={() => setName(undefined)}
                inputOtherProps={{ placeholder: "Name..." }}
                showInvalidText={isAllValid === false}
              />
              {nameIsFree == false && (
                <div className="error-text">{nameIsOccupiedText}</div>
              )}
              <h2 className="mt-5">Description:</h2>
              <TextAreaComponent
                id="description"
                invalidText={descriptionInvalidText}
                validatorFun={descriptionWithEmptyValidator}
                afterValidationFun={(e) => {
                  setDescription(e.target.value || "");
                }}
                invalidatedFun={() => setDescription(undefined)}
                textareaOtherProps={{
                  placeholder: "Description...",
                  maxlength: 499,
                }}
                showInvalidText={isAllValid === false}
              />
              <h2 className="mt-5">Price:</h2>
              <div className="product-category-create-or-update-price">
                <InputComponent
                  id="price"
                  type="number"
                  invalidText={priceInvalidText}
                  validatorFun={greaterThanZeroValidator}
                  afterValidationFun={(e) => {
                    setPrice(e.target.value);
                  }}
                  invalidatedFun={() => setPrice(undefined)}
                  inputOtherProps={{ placeholder: "Price..." }}
                  showInvalidText={isAllValid === false}
                />
              </div>
              <h2 className="mt-5">Quantity:</h2>
              <div className="product-category-quantity-input">
                <InputComponent
                  id="quantity"
                  type="number"
                  invalidText={quantityInvalidText}
                  validatorFun={greaterThanZeroValidator}
                  afterValidationFun={(e) => {
                    setQuantity(e.target.value);
                  }}
                  invalidatedFun={() => setQuantity(undefined)}
                  inputOtherProps={{ placeholder: "Quantity..." }}
                  showInvalidText={isAllValid === false}
                />
              </div>
              <h2 className="mt-5">Delivery company:</h2>
              <SelectDeliveryCompanyComponent
                selectedCompany={deliveryCompany}
                selectFun={(selectedCompany) =>
                  setDeliveryCompany(selectedCompany)
                }
                showInvalidText={isAllValid === false}
              />
              <h2 className="mt-5">Tags:</h2>
              <AddTagsComponent
                tags={tags}
                addTagFun={(newTag) => setTags([...tags, newTag])}
                deleteTagFun={(deletedTag) => {
                  setTags(tags.filter((x) => x != deletedTag));
                }}
                showInvalidText={isAllValid === false}
              />
              <h2 className="mt-5">Images:</h2>
              <ChooseImagesComponent
                images={images}
                changeImagesFun={(newImages) => {
                  console.log("new Images: ", newImages);
                  setImages([...newImages]);
                }}
                showInvalidText={isAllValid === false}
                buttonText={"Choose"}
              />
              <button
                className="product-category-create-or-update-button mt-32"
                onClick={async () => {
                  let isValid =
                    greaterThanZeroValidator(quantity) &&
                    deliveryCompany &&
                    images.length > 0 &&
                    guidValidator(deliveryCompany.id) &&
                    greaterThanZeroValidator(price) &&
                    tags.length > 0 &&
                    descriptionValidator(description) &&
                    categoryNameValidator(name);

                  setIsAllValid(isValid);

                  let isFree = await nameIsFreeCheck(name);
                  setNameIsFree(isFree);
                  if (isValid === true && isFree === true) await sent();
                }}
              >
                Create
              </button>
            </div>
          </div>
        );
    }
  }
  return stringToBool(isCustomer) == true ? (
    <div>You is not seller</div>
  ) : (
    render()
  );
}
