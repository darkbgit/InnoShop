import { isRouteErrorResponse, Link, useRouteError } from "react-router";

const ErrorPage = () => {
  const error = useRouteError();
  console.error(error);
  return (
    <>
      <div className="error-container">
        {isRouteErrorResponse(error) ? (
          <>
            <h1>{error.status}</h1>
            <p>{error.statusText}</p>
            {error.data?.message && <p>{error.data.message}</p>}
          </>
        ) : (
          <>
            <h1>Oops!</h1>
            <p>Something went wrong.</p>
            <p>
              <i>{(error as Error).message || "Unknown Error"}</i>
            </p>
          </>
        )}
      </div>
      <Link to="/">Go to Home Page</Link>
    </>
  );
};

export default ErrorPage;
