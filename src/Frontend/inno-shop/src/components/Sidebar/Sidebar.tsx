import {
  Button,
  Input,
  Checkbox,
  FormControlLabel,
  Box,
  IconButton,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  type SelectChangeEvent,
  Stack,
} from "@mui/material";
import {
  Link,
  useNavigate,
  useRouteLoaderData,
  useSearchParams,
} from "react-router";
import type { UserInfo } from "../../interfaces/user.interface";
import { useState } from "react";
import ClearIcon from "@mui/icons-material/Clear";

const Sidebar = () => {
  const currentUser = useRouteLoaderData("root") as UserInfo | null;
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [searchTerm, setSearchTerm] = useState(
    searchParams.get("searchString") || ""
  );
  const [sortOption, setSortOption] = useState(
    `${searchParams.get("sortBy") || "CreatedDate"}_${
      searchParams.get("sortOrder") || "desc"
    }`
  );

  const [onlyMyProducts, setOnlyMyProducts] = useState(
    !!searchParams.get("createdBy")
  );

  const handleOnlyMyProductsChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { checked } = event.target;
    setOnlyMyProducts(checked);
    const params = new URLSearchParams(searchParams);
    if (checked) {
      if (currentUser) {
        params.set("createdBy", currentUser.id);
      }
    } else {
      params.delete("createdBy");
    }
    navigate(`/?${params.toString()}`);
  };

  const handleSearch = () => {
    const params = new URLSearchParams(searchParams);
    params.set("searchString", searchTerm);
    navigate(`/?${params.toString()}`);
  };

  const handleClearSearch = () => {
    setSearchTerm("");
    const params = new URLSearchParams(searchParams);
    params.delete("searchString");
    navigate(`/?${params.toString()}`);
  };

  const handleSortChange = (event: SelectChangeEvent<string>) => {
    const { value } = event.target;
    setSortOption(value);
    const [sortBy, sortOrder] = value.split("_");

    const params = new URLSearchParams(searchParams);
    params.set("sortBy", sortBy);
    params.set("sortOrder", sortOrder);
    navigate(`/?${params.toString()}`);
  };

  const sortOptions = [
    { value: "Name_ascending", label: "Name A->Z" },
    { value: "Name_descending", label: "Name Z->A" },
    { value: "Price_ascending", label: "Price (low to high)" },
    { value: "Price_descending", label: "Price (high to low)" },
    { value: "CreatedDate_ascending", label: "Created Date (oldest first)" },
    { value: "CreatedDate_descending", label: "Created Date (newest first)" },
  ];

  return (
    <Box p={2}>
      <Stack direction="row" spacing={2} alignItems="center">
        <Box sx={{ display: "flex", alignItems: "center" }}>
          <Input
            placeholder="Search..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                handleSearch();
              }
            }}
          />
          {searchTerm && (
            <IconButton onClick={handleClearSearch} size="small">
              <ClearIcon />
            </IconButton>
          )}
        </Box>
        <FormControl sx={{ minWidth: 120 }}>
          <InputLabel id="sort-by-label">Sort By</InputLabel>
          <Select
            labelId="sort-by-label"
            id="sort-by-select"
            value={sortOption}
            label="Sort By"
            onChange={handleSortChange}
          >
            {sortOptions.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        {currentUser && (
          <div style={{ display: "flex", alignItems: "center" }}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={onlyMyProducts}
                  onChange={handleOnlyMyProductsChange}
                />
              }
              label="Only My products"
            ></FormControlLabel>
            <Button>
              <Link to="product/new">New</Link>
            </Button>
          </div>
        )}
      </Stack>
    </Box>
  );
};

export default Sidebar;
