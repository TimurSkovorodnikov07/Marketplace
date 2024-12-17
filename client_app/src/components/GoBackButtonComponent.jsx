import { useNavigate } from "react-router-dom";

export function GoBackButtonComponent() {
  const navigate = useNavigate();

  return (
    <a
      href="#"
      onClick={(e) => {
        console.log(e);
        navigate(-1);
      }}
    >
      <span>Go back</span>
    </a>
  );
}
