import { useEffect, useState } from "react";
import {
  getCategory,
  productCategoryUpdate,
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
import { useNavigate, useParams } from "react-router-dom";
import { greaterThanZeroValidator } from "../validators/greaterThanZeroValidator";
import { AddTagsComponent } from "../components/AddTagsComponent";
import { SelectDeliveryCompanyComponent } from "../components/SelectDeliveryCompanyComponent";
import { guidValidator } from "../validators/guidValidator";
import { nameIsFreeCheck } from "./ProductCategoryCreatePage";
import { TextAreaComponent } from "../components/TextAreaComponent";

export function ProductCategoryUpdate() {
  const categoryId = useParams().id;
  const [foundCategory, setFoundCategory] = useState(undefined);
  const [codeAndText, setCodeAndText] = useResponseCode();
  const navigate = useNavigate();

  const isCustomer = useSelector((state) => state.auth.isCustomer);
  const [isAllValid, setIsAllValid] = useState(true);
  const [isUpdated, setIsUpdated] = useState(false);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getCategory(categoryId);
        if (response.status === 200) {
          const data = response.data;
          setFoundCategory(data);

          setName(data.name);
          setDescription(data.description);
          setTags(data.tags);
          setPrice(data.price);
          setQuantity(data.quantity);
        }
      } catch (error) {
        console.error(error);
      }
    };
    asyncFun();
  }, []);
  const [newName, setName] = useState(undefined);
  const [nameIsFree, setNameIsFree] = useState(true);
  const [newDescription, setDescription] = useState(undefined);
  const [newTags, setTags] = useState([]);
  const [newPrice, setPrice] = useState(undefined);
  const [newQuantity, setQuantity] = useState(undefined);
  const [newDeliveryCompany, setDeliveryCompany] = useState(undefined);

  function comeBack() {
    navigate(`/products/${categoryId}`);
  }
  async function update() {
    try {
      const response = await productCategoryUpdate(
        categoryId,
        newName,
        newDescription,
        newTags,
        newPrice,
        newQuantity,
        newDeliveryCompany.id
      );
      setCodeAndText({
        code: response.status,
        text: response.data,
      });
      if (response.status === 200) comeBack();
    } catch (error) {
      console.error(error);
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
        return <div>You not owner</div>;
      default:
        return foundCategory ? (
          <div className="product-category-create-or-update-page-wrapper">
            <h1 className="pb-10">Update the category</h1>
            <div className="product-category-create-or-update-wrapper">
              <h2>Name:</h2>
              <InputComponent
                id="name"
                type="text"
                invalidText={nameInvalidText}
                validatorFun={categoryNameValidator}
                afterValidationFun={(e) => {
                  setName(e.target.value);
                }}
                invalidatedFun={() => setName(undefined)}
                inputOtherProps={{
                  placeholder: "Name...",
                }}
                defaultValue={foundCategory.name}
                showInvalidText={isAllValid === false}
                beforeValidationFun={(e) => {
                  setIsUpdated(true);
                  setNameIsFree(true);
                }}
              />

              <h2 className="mt-5">Description:</h2>
              <TextAreaComponent
                id="description"
                type="text"
                invalidText={descriptionInvalidText}
                validatorFun={descriptionWithEmptyValidator}
                afterValidationFun={(e) => {
                  setDescription(e.target.value || "");
                }}
                invalidatedFun={() => setDescription(undefined)}
                inputOtherProps={{
                  placeholder: "Description...",
                }}
                defaultValue={foundCategory.description}
                showInvalidText={isAllValid === false}
                beforeValidationFun={(e) => setIsUpdated(true)}
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
                  inputOtherProps={{
                    placeholder: "Price...",
                  }}
                  defaultValue={foundCategory.price}
                  showInvalidText={isAllValid === false}
                  beforeValidationFun={(e) => setIsUpdated(true)}
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
                    inputOtherProps={{
                      placeholder: "Quantity...",
                    }}
                    defaultValue={foundCategory.quantity}
                    showInvalidText={isAllValid === false}
                    beforeValidationFun={(e) => setIsUpdated(true)}
                  />
              </div>
              <h2 className="mt-5">Delivery company:</h2>
              <SelectDeliveryCompanyComponent
                selectedCompany={newDeliveryCompany}
                selectFun={(selectedCompany) =>
                  setDeliveryCompany(selectedCompany)
                }
                showInvalidText={isAllValid === false}
                defaultCompanyId={foundCategory.deliveryCompanyId}
                onChanged={(e) => setIsUpdated(true)}
                showCompaniesAtTheBegin={true}
              />
              <h2 className="mt-5">Tags:</h2>
              <AddTagsComponent
                tags={newTags}
                addTagFun={(newTag) => setTags([...newTags, newTag])}
                deleteTagFun={(deletedTag) => {
                  setTags(newTags.filter((x) => x != deletedTag));
                }}
                showInvalidText={isAllValid === false}
                onChanged={(e) => setIsUpdated(true)}
              />
              <button
              className="product-category-create-or-uptate-button mt-32"
                onClick={async () => {
                  let isValid =
                    greaterThanZeroValidator(newQuantity) &&
                    newDeliveryCompany &&
                    guidValidator(newDeliveryCompany?.id) &&
                    greaterThanZeroValidator(newPrice) &&
                    newTags.length > 0 &&
                    descriptionValidator(newDescription) &&
                    categoryNameValidator(newName);

                  setIsAllValid(isValid);

                  let isFree = await nameIsFreeCheck(newName);
                  setNameIsFree(isFree);

                  if (
                    isValid === true &&
                    isUpdated === true &&
                    isFree === true
                  ) {
                    console.log("Changed");
                    await update();
                  } else if (isUpdated === false) {
                    console.log("Seller didn't changed this product category");
                    comeBack();
                  }
                }}
              >
                Come back and save
              </button>
            </div>
          </div>
        ) : (
          <div>Category not found</div>
        );
    }
  }
  return stringToBool(isCustomer) == true ? (
    <div>You is not seller</div>
  ) : (
    render()
  );
}
