import { Link } from "react-router-dom";

export function ToolTip({linkTo, linkText, children}) {
  return (
    <>
      <span className="tooltip">
        <Link to={linkTo}>{linkText}</Link>
        <div className="tooltiptext small-text">{children}</div>
      </span>
    </>
  );
}
