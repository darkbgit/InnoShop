import { Button } from "@mui/material";
import { Link, useFetcher, useRouteLoaderData } from "react-router";
import type { User } from "../../interfaces/user.interface";

const LoginMenu = () => {
  const user = useRouteLoaderData("root") as User | null;
  const fetcher = useFetcher();

  if (user) {
    return (
      <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
        <span>{user.email}</span>
        <fetcher.Form method="post" action="/logout">
          <Button type="submit" color="inherit">
            Logout
          </Button>
        </fetcher.Form>
      </div>
    );
  }

  return (
    <div style={{ display: "flex", gap: "1rem" }}>
      <Button color="inherit" component={Link} to="/login">
        Login
      </Button>
      <Button color="inherit" component={Link} to="/register">
        Sign Up
      </Button>
    </div>
  );
};

export default LoginMenu;
