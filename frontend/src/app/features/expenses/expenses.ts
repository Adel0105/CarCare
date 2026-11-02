import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { CreateExpense, Expense, ExpenseCategory } from './expense';
import { ExpenseService } from './expense.service';

type ExpensesViewState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'success'; expenses: Expense[] };

interface ExpenseFormModel {
  date: string;
  amount: number | null;
  category: ExpenseCategory;
  description: string;
  mileage: number | null;
}

interface FormMessage {
  type: 'success' | 'error';
  text: string;
}

@Component({
  selector: 'app-expenses',
  imports: [FormsModule],
  templateUrl: './expenses.html',
  styleUrl: './expenses.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Expenses implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly expenseService = inject(ExpenseService);

  protected readonly ExpenseCategory = ExpenseCategory;

  protected readonly form: ExpenseFormModel = {
    date: '',
    amount: null,
    category: ExpenseCategory.Fuel,
    description: '',
    mileage: null,
  };

  private vehicleId: number | null = null;

  protected readonly viewState = signal<ExpensesViewState>({ status: 'loading' });
  protected readonly formMessage = signal<FormMessage | null>(null);
  protected readonly isSubmitting = signal(false);
  protected readonly deletingExpenseId = signal<number | null>(null);
  protected readonly editingExpense = signal<Expense | null>(null);

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.vehicleId = Number(params.get('vehicleId'));
      this.loadExpenses();
    });
  }

  protected startEditing(expense: Expense): void {
    this.editingExpense.set(expense);
    this.form.date = expense.date;
    this.form.amount = expense.amount;
    this.form.category = expense.category;
    this.form.description = expense.description ?? '';
    this.form.mileage = expense.mileage;
    this.formMessage.set(null);
  }

  protected cancelEditing(): void {
    this.editingExpense.set(null);
    this.resetForm();
    this.formMessage.set(null);
  }

  protected deleteExpense(expense: Expense): void {
    if (this.deletingExpenseId() !== null || this.isSubmitting()) {
      return;
    }

    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.formMessage.set({
        type: 'error',
        text: 'Greška pri brisanju troška. Pokušaj ponovo.',
      });
      return;
    }

    const confirmed = confirm(
      `Jesi li siguran da želiš obrisati trošak „${this.categoryLabel(expense.category)}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.deletingExpenseId.set(expense.id);
    this.formMessage.set(null);

    this.expenseService.delete(this.vehicleId, expense.id).subscribe({
      next: () => {
        if (this.editingExpense()?.id === expense.id) {
          this.cancelEditing();
        }

        this.deletingExpenseId.set(null);
        this.formMessage.set({
          type: 'success',
          text: 'Trošak je uspješno obrisan.',
        });
        this.loadExpenses();
      },
      error: () => {
        this.deletingExpenseId.set(null);
        this.formMessage.set({
          type: 'error',
          text: 'Greška pri brisanju troška. Pokušaj ponovo.',
        });
      },
    });
  }

  protected onSubmit(): void {
    if (this.isSubmitting() || this.deletingExpenseId() !== null) {
      return;
    }

    if (
      !this.form.date ||
      this.form.amount === null ||
      this.form.amount <= 0 ||
      !this.isValidCategory(this.form.category) ||
      (this.form.mileage !== null && this.form.mileage < 0)
    ) {
      this.formMessage.set({
        type: 'error',
        text: 'Popuni sva obavezna polja ispravno.',
      });
      return;
    }

    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.formMessage.set({
        type: 'error',
        text: 'Greška pri dodavanju troška. Pokušaj ponovo.',
      });
      return;
    }

    const payload: CreateExpense = {
      date: this.form.date,
      amount: this.form.amount,
      category: this.form.category,
      description: this.form.description.trim() ? this.form.description.trim() : null,
      mileage: this.form.mileage,
    };

    const editing = this.editingExpense();

    this.isSubmitting.set(true);
    this.formMessage.set(null);

    if (editing) {
      this.expenseService.update(this.vehicleId, editing.id, payload).subscribe({
        next: () => {
          this.editingExpense.set(null);
          this.resetForm();
          this.isSubmitting.set(false);
          this.formMessage.set({
            type: 'success',
            text: 'Trošak je uspješno ažuriran.',
          });
          this.loadExpenses();
        },
        error: () => {
          this.isSubmitting.set(false);
          this.formMessage.set({
            type: 'error',
            text: 'Greška pri ažuriranju troška. Pokušaj ponovo.',
          });
        },
      });
      return;
    }

    this.expenseService.create(this.vehicleId, payload).subscribe({
      next: () => {
        this.resetForm();
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'success',
          text: 'Trošak je uspješno dodan.',
        });
        this.loadExpenses();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.formMessage.set({
          type: 'error',
          text: 'Greška pri dodavanju troška. Pokušaj ponovo.',
        });
      },
    });
  }

  protected categoryLabel(category: ExpenseCategory): string {
    switch (category) {
      case ExpenseCategory.Fuel:
        return 'Gorivo';
      case ExpenseCategory.Insurance:
        return 'Osiguranje';
      case ExpenseCategory.Registration:
        return 'Registracija';
      case ExpenseCategory.Parking:
        return 'Parking';
      case ExpenseCategory.Other:
        return 'Ostalo';
    }
  }

  private loadExpenses(): void {
    if (this.vehicleId === null || Number.isNaN(this.vehicleId)) {
      this.viewState.set({ status: 'error' });
      return;
    }

    this.viewState.set({ status: 'loading' });

    this.expenseService.getByVehicleId(this.vehicleId).subscribe({
      next: (expenses) => {
        this.viewState.set({ status: 'success', expenses });
      },
      error: () => {
        this.viewState.set({ status: 'error' });
      },
    });
  }

  private resetForm(): void {
    this.form.date = '';
    this.form.amount = null;
    this.form.category = ExpenseCategory.Fuel;
    this.form.description = '';
    this.form.mileage = null;
  }

  private isValidCategory(category: ExpenseCategory): boolean {
    return (
      category === ExpenseCategory.Fuel ||
      category === ExpenseCategory.Insurance ||
      category === ExpenseCategory.Registration ||
      category === ExpenseCategory.Parking ||
      category === ExpenseCategory.Other
    );
  }
}
