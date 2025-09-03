export default function ErrorMessage({ message, httpCode }) {
  return httpCode == 0 ? (
    <div className="error-text">Error: {message}</div>
  ) : (
    <div>
      <p>
        Http code: <strong className="error-text">{httpCode}</strong>
      </p>
      <p>
        Message: <strong className="error-text">{message}</strong>
      </p>
    </div>
  );
}
