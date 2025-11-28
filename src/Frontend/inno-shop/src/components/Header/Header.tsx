import { Box } from "@mui/material";
import LoginMenu from "../LoginMenu/LoginMenu";
import { Link, useRouteLoaderData } from "react-router";
import type { UserInfo } from "../../interfaces/user.interface";

const Header = () => {
  const user = useRouteLoaderData("root") as UserInfo | null;
  return (
    <div>
      <Box p={2}>
        <nav style={{ display: "flex", gap: "1rem", alignItems: "center" }}>
          <Link to="/">Home</Link>
          <Link to="/products">Products</Link>
          {user && user.roles.includes("Admin") && (
            <Link to="/users">Users</Link>
          )}
          <LoginMenu />
        </nav>
      </Box>
      <hr />
    </div>
  );
};

export default Header;
