import { makeAutoObservable, runInAction } from "mobx";
import eventAgent from "../helpers/api/eventAgent";
import {
  CreateEventModel,
  EventArea,
  EventAreaWithTotalSeats,
  EventEditListModel,
  EventModel,
  EventSeat,
  EventWithLayouts,
  PageInfo,
} from "../interfaces/event.interface";

export default class ProductStore {
  pageInfo: PageInfo = {
    currentPage: 0,
    pageSize: 0,
    totalCount: 0,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  };

  productsEditRegistry = new Map<number, EventEditListModel>();
  selectedEvent: EventWithLayouts | undefined = undefined;
  createProductMode = false;
  editProductMode = false;

  loading = false;
  loadingInitial = true;

  constructor() {
    makeAutoObservable(this);
  }

  get productsEditList() {
    return Array.from(this.productsEditRegistry.values());
  }

  setProduct = (event: EventEditListModel) => {
    event.startDate = new Date(event.startDate);
    this.productsEditRegistry.set(event.id, event);
  };

  loadProducts = async (params: URLSearchParams) => {
    try {
      const pagedProducts = await eventAgent.Products.editList(params);
      runInAction(() => {
        const pageInfo: PageInfo = pagedProducts as PageInfo;
        this.pageInfo = pageInfo;
        this.productsEditRegistry.clear();
        pagedProducts.items.forEach((product) => {
          this.setProduct(product);
        });
        this.loadingInitial = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  updateProduct = async (event: EventModel) => {
    this.loading = true;
    try {
      await eventAgent.Events.update(event);
      runInAction(() => {
        if (event.id) {
          let updatedEvent = {
            ...this.productsEditRegistry.get(event.id),
            ...event,
          };
          this.productsEditRegistry.set(
            event.id,
            updatedEvent as EventEditListModel
          );
        }
        this.selectedEvent = undefined;
        this.editEventMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  createProduct = async (event: CreateEventModel) => {
    this.loading = true;
    try {
      await eventAgent.Events.create(event);
      runInAction(() => {
        this.createEventMode = false;
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  deleteProduct = async (id: number) => {
    this.loading = true;
    try {
      await eventAgent.Events.delete(id);
      runInAction(() => {
        this.productsEditRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  selectProduct = async (id: number) => {
    this.loading = true;
    try {
      const event = await eventAgent.Events.withLayouts(id);
      runInAction(() => {
        this.selectedEvent = event;
        this.loading = false;
      });
      return event;
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setSelectedProduct = (event: EventWithLayouts) => {
    this.selectedEvent = event;
  };

  setCreateProductMode = (state: boolean) => {
    this.createEventMode = state;
  };

  cancelSelectProduct = () => {
    this.selectedEvent = undefined;
  };

  openEditForm = async (id: number) => {
    await this.selectProduct(id);
    runInAction(() => {
      this.editEventMode = true;
    });
  };

  closeEditForm = () => {
    this.cancelSelectProduct();
    this.editEventMode = false;
  };
}
