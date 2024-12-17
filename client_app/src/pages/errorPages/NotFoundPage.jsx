import ErrorMessage from "../../components/ErrorMessage";

export default function NotFoundPage() {
  return (
    <>
      <ErrorMessage httpCode={404} message="Page not found" />
    </>
  );
}
