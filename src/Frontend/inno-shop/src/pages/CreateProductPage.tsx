import {
  Alert,
  Button,
  TextField,
  Typography,
  Container,
  Box,
  Checkbox,
  FormControlLabel,
} from "@mui/material";
import { Form, Link, useActionData, useNavigation } from "react-router";
import { useState } from "react";

const CreateProductPage = () => {
  const navigation = useNavigation();
  const actionData = useActionData() as { error: string } | undefined;
  const isSubmitting = navigation.state === "submitting";

  const [isAvailable, setIsAvailable] = useState(true);
  const [isOnSale, setIsOnSale] = useState(false);

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
          Create Product
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
            autoFocus
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="summary"
            label="Summary"
            name="summary"
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
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="price"
            label="Price"
            name="price"
          />
          <TextField
            margin="normal"
            fullWidth
            id="salePrice"
            label="Sale Price"
            name="salePrice"
            disabled={!isOnSale}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="categoryId"
            label="Category ID"
            name="categoryId"
            type="number"
          />
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
              {isSubmitting ? "Creating..." : "Create Product"}
            </Button>
            <Button
              component={Link}
              to="/"
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

export default CreateProductPage;
