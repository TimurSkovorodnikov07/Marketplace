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
import { NumberInputComponent } from "../components/NumberInputComponent";

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
          <div className="product-category-create-wrapper">
            <InputComponent
              id="name"
              type="text"
              labelText="Name: "
              invalidText={
                nameIsFree === true ? nameInvalidText : "Name is occupied"
              }
              validatorFun={(value) => {
                return (
                  categoryNameValidator(value) === true && nameIsFree === true
                );
              }}
              afterValidationFun={(e) => {
                setName(e.target.value);
              }}
              beforeValidationFun={(e) => setNameIsFree(true)}
              invalidatedFun={() => setName(undefined)}
              inputOtherProps={{ placeholder: "Name..." }}
              showInvalidText={isAllValid === false}
            />

            <TextAreaComponent
              id="description"
              labelText="Descipriton: "
              invalidText={descriptionInvalidText}
              validatorFun={descriptionWithEmptyValidator}
              afterValidationFun={(e) => {
                setDescription(e.target.value || "");
              }}
              invalidatedFun={() => setDescription(undefined)}
              inputOtherProps={{
                placeholder: "Description...",
                maxlength: 500,
              }}
              showInvalidText={isAllValid === false}
            />

            <AddTagsComponent
              tags={tags}
              labelText="Tags: "
              addTagFun={(newTag) => setTags([...tags, newTag])}
              deleteTagFun={(deletedTag) => {
                setTags(tags.filter((x) => x != deletedTag));
              }}
              showInvalidText={isAllValid === false}
            />

            <InputComponent
              id="price"
              type="number"
              labelText="Price: "
              invalidText={priceInvalidText}
              validatorFun={greaterThanZeroValidator}
              afterValidationFun={(e) => {
                setPrice(e.target.value);
              }}
              invalidatedFun={() => setPrice(undefined)}
              inputOtherProps={{ placeholder: "Price..." }}
              showInvalidText={isAllValid === false}
            />

            <SelectDeliveryCompanyComponent
              selectedCompany={deliveryCompany}
              labelText="Delivery company: "
              selectFun={(selectedCompany) =>
                setDeliveryCompany(selectedCompany)
              }
              showInvalidText={isAllValid === false}
            />

            <ChooseImagesComponent
              labelText="Images(1x1): "
              images={images}
              changeImagesFun={(newImage) => setImages([...newImage])}
              showInvalidText={isAllValid === false}
            />
            <InputComponent
              id="quantity"
              labelText="Quantity: "
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
            <button
              onClick={async () => {
                const isValid =
                  greaterThanZeroValidator(quantity) &&
                  deliveryCompany &&
                  images.length > 0 &&
                  guidValidator(deliveryCompany.id) &&
                  greaterThanZeroValidator(price) &&
                  tags.length > 0 &&
                  descriptionValidator(description) &&
                  categoryNameValidator(name);

                setIsAllValid(isValid);

                const isFree = await nameIsFreeCheck(name);
                setNameIsFree(isFree);
                if (isValid === true && isFree === true) await sent();
              }}
            >
              Create
            </button>
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
