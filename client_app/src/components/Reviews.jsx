import { useEffect, useState } from "react";
import { getMyReview, getReviews } from "../requests/reviewRequests";
import { paginationInterval } from "../configs/pagination";
import { ReviewWrite } from "./ReviewWrite";
import { useSelector } from "react-redux";
import { MyReview } from "./MyReview";
import { ReviewComponent } from "./ReviewComponent";

export function Reviews({ categoryId, canWrite = true }) {
  const isAuth = useSelector((state) => state.auth.isAuth);
  const [reviews, setReviews] = useState([]);
  const [myReview, setMyReview] = useState(undefined);
  const [from, setFrom] = useState(0);
  const [maxCount, setMaxCount] = useState(0);

  const [text, setText] = useState("");
  const [estimation, setEstimation] = useState(0);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getMyReview(categoryId);
        if (response.status === 200) {
          const data = response.data;
          setMyReview(data);
        }
      } catch (error) {
        console.error(error);
      }
    };
    asyncFun();
  }, [categoryId]);

  useEffect(() => {
    const asyncFun = async () => {
      try {
        const response = await getReviews(
          categoryId,
          from,
          from + paginationInterval
        );
        if (response.status === 200) {
          setReviews([...reviews, ...response.data]);
          setMaxCount(parseInt(response.headers["x-max-count"]));
        }
      } catch (error) {
        console.error(error);
      }
    };
    asyncFun();
  }, [categoryId, from]);

  return (
    <>
      {isAuth === true ? (
        <>
          {myReview === undefined && canWrite === true && (
            <div className="write-review">
              <ReviewWrite
                categoryId={categoryId}
                text={text}
                estimation={estimation}
                setText={setText}
                setEstimation={setEstimation}
              />
            </div>
          )}
          <ul>
            {myReview !== undefined && (
              <MyReview
                review={myReview}
                text={text}
                estimation={estimation}
                setText={setText}
                setEstimation={setEstimation}
              />
            )}
            {reviews.map((x) => (
              <li>
                <ReviewComponent review={x} />
              </li>
            ))}
          </ul>
          {from + paginationInterval < maxCount && (
            <button onClick={() => setFrom(from + paginationInterval)}>
              More...
            </button>
          )}
        </>
      ) : (
        <></>
      )}
    </>
  );
}
