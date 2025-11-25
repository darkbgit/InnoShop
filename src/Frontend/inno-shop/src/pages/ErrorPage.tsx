import { isRouteErrorResponse, useRouteError } from "react-router";

const ErrorPage = () => {
  const error = useRouteError();
  console.error(error);
  if (isRouteErrorResponse(error)) {
    return (
      <div className="error-container">
        <h1>{error.status}</h1>
        <p>{error.statusText}</p>
        {error.data?.message && <p>{error.data.message}</p>}
      </div>
    );
  }

  return (
    <div className="error-container">
      <h1>Oops!</h1>
      <p>Something went wrong.</p>
      <p>
        <i>{error.message || "Unknown Error"}</i>
      </p>
    </div>
  );
};

export default ErrorPage;
