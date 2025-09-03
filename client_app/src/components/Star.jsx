import { FaStar } from "react-icons/fa";

export function Star({ isHover, className = "star" }) {
  return (
    <FaStar
      className={className}
      color={isHover === true ? "#f9e2af" : "#6c7086"}
    />
  );
}
