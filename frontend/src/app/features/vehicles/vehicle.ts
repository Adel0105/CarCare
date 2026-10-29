export interface Vehicle {
  id: number;
  make: string;
  model: string;
  year: number;
  licensePlate: string;
  currentMileage: number;
  notes: string | null;
}

export interface CreateVehicle {
  make: string;
  model: string;
  year: number;
  licensePlate: string;
  currentMileage: number;
  notes: string | null;
}
