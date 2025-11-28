import {
  Alert,
  Button,
  TextField,
  Typography,
  Container,
  Box,
  Checkbox,
  FormControlLabel,
  InputLabel,
  Select,
  MenuItem,
  FormControl,
} from "@mui/material";
import {
  Form,
  Link,
  useActionData,
  useLoaderData,
  useLocation,
  useNavigation,
} from "react-router";
import { useState } from "react";
import type { Category, ProductEdit } from "../interfaces/product.interface";

const EditProductPage = () => {
  const { product, categories } = useLoaderData() as {
    product: ProductEdit;
    categories: Category[];
  };
  const location = useLocation();
  const prevSearch = location.state?.prevSearch || "";
  const navigation = useNavigation();
  const actionData = useActionData() as { error: string } | undefined;
  const isSubmitting = navigation.state === "submitting";

  const [isAvailable, setIsAvailable] = useState(product.isAvailable);
  const [isOnSale, setIsOnSale] = useState(product.isOnSale);

  return (
    <Container maxWidth="sm">
      <Box
        sx={{
          marginTop: 4,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Typography component="h1" variant="h5">
          Edit Product
        </Typography>
        {actionData && actionData.error && (
          <Alert severity="error" sx={{ mt: 2, width: "100%" }}>
            {actionData.error}
          </Alert>
        )}
        <Form method="post" style={{ width: "100%" }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="name"
            label="Product Name"
            name="name"
            defaultValue={product.name}
            autoFocus
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="summary"
            label="Summary"
            name="summary"
            defaultValue={product.summary}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="description"
            label="Description"
            name="description"
            multiline
            rows={4}
            defaultValue={product.description}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="price"
            label="Price"
            name="price"
            defaultValue={product.price}
          />
          <TextField
            margin="normal"
            fullWidth
            id="salePrice"
            label="Sale Price"
            name="salePrice"
            disabled={!isOnSale}
            defaultValue={product.salePrice}
          />
          <FormControl fullWidth>
            <InputLabel id="category-label">Category</InputLabel>
            <Select
              labelId="category-label"
              fullWidth
              id="categoryId"
              name="categoryId"
              defaultValue={product.categoryId}
              label="Category"
            >
              {categories.map((option) => (
                <MenuItem key={option.id} value={option.id}>
                  {option.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <FormControlLabel
            control={
              <Checkbox
                checked={isAvailable}
                onChange={(e) => setIsAvailable(e.target.checked)}
                name="isAvailable"
                color="primary"
              />
            }
            label="Is Available"
          />
          <FormControlLabel
            control={
              <Checkbox
                checked={isOnSale}
                onChange={(e) => setIsOnSale(e.target.checked)}
                name="isOnSale"
                color="primary"
              />
            }
            label="Is on Sale"
          />
          <Box sx={{ display: "flex", gap: 2, mt: 3, mb: 2 }}>
            <Button
              type="submit"
              fullWidth
              variant="contained"
              disabled={isSubmitting}
            >
              {isSubmitting ? "Updating..." : "Update Product"}
            </Button>
            <Button
              component={Link}
              to={`/products${prevSearch}`}
              fullWidth
              variant="outlined"
              color="secondary"
            >
              Cancel
            </Button>
          </Box>
        </Form>
      </Box>
    </Container>
  );
};

export default EditProductPage;
