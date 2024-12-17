import { useEffect, useState } from "react";
import { Star } from "./Star";

export function Stars({
  estimation,
  setEstimation,
  estimationCount,
  starClassName = "star",
  defaultValue = 0,
}) {
  const starsNumber = 10;
  const [hover, setHover] = useState();
  const array = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

  useEffect(() => {
    if (defaultValue >= 0 && defaultValue <= 10 && estimation == 0) {
      setEstimation?.(defaultValue);
    }
  }, [estimation, defaultValue]);

  return (
    <div className="inline-block ">
      <ul className="starrate">
        {array.map((x, index) => (
          <li key={x}>
            {setEstimation === undefined ? (
              <Star className={starClassName} isHover={estimation > index} />
            ) : (
              <button
                className="star-button"
                onClick={() => setEstimation(index + 1)}
                onMouseEnter={() => setHover(index + 1)}
                onMouseLeave={() => setHover(null)}
              >
                <Star
                  isHover={
                    estimation > hover
                      ? index + 1 <= estimation
                      : index + 1 <= hover
                  }
                />
              </button>
            )}
          </li>
        ))}
        {estimationCount !== undefined && (
          <li className={starClassName}>({estimationCount})</li>
        )}
      </ul>
    </div>
  );
}
