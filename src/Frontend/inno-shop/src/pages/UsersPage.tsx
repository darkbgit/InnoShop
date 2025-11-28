import { useLoaderData, Form, useSearchParams } from "react-router";
import type { User } from "../interfaces/user.interface";
import type { usersLoader } from "../loaders/authLoaders";
import { List, ListItem, Pagination, Stack } from "@mui/material";

const UsersPage = () => {
  const paginatedUsers = useLoaderData<typeof usersLoader>();
  const [searchParams, setSearchParams] = useSearchParams();

  const handlePageChange = (
    event: React.ChangeEvent<unknown>,
    value: number
  ) => {
    setSearchParams((prevParams) => {
      prevParams.set("pageNumber", value.toString());
      return prevParams;
    });
  };

  const users: User[] = paginatedUsers.items;

  return (
    <div>
      <h1>Users</h1>
      <List>
        {users.map((user) => (
          <ListItem key={user.id}>
            {user.email}
            <Form
              method="post"
              action={`/users/${user.id}/delete`}
              style={{ display: "inline", marginLeft: "1rem" }}
            >
              <button type="submit">Delete</button>
            </Form>
          </ListItem>
        ))}
      </List>
      <Stack spacing={2} sx={{ alignItems: "center", marginTop: 4 }}>
        <Pagination
          count={paginatedUsers.totalPages}
          page={paginatedUsers.currentPage}
          onChange={handlePageChange}
        />
      </Stack>
    </div>
  );
};

export default UsersPage;
