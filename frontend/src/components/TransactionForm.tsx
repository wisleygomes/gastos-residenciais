import { useState, FormEvent, useMemo } from 'react';
import { TransactionType } from '../types';
import type { CreateTransactionInput, Person } from '../types';

interface TransactionFormProps {
  people: Person[];
  onSubmit: (input: CreateTransactionInput) => Promise<void>;
}

/**
 * Formulário controlado para cadastro de uma nova transação.
 * Quando a pessoa selecionada é menor de idade, a opção "Receita" é
 * desabilitada no próprio formulário (espelhando a regra de negócio
 * que também é validada, de forma definitiva, no backend).
 */
export default function TransactionForm({ people, onSubmit }: TransactionFormProps) {
  const [description, setDescription] = useState('');
  const [value, setValue] = useState('');
  const [type, setType] = useState<TransactionType>(TransactionType.Expense);
  const [personId, setPersonId] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const selectedPerson = useMemo(
    () => people.find((p) => p.id === personId),
    [people, personId]
  );

  const incomeDisabled = selectedPerson?.isMinor ?? false;

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError(null);

    const parsedValue = Number(value);

    if (!personId) {
      setError('Selecione a pessoa responsável pela transação.');
      return;
    }
    if (!description.trim()) {
      setError('Informe uma descrição para a transação.');
      return;
    }
    if (!(parsedValue > 0)) {
      setError('Informe um valor maior que zero.');
      return;
    }

    setSubmitting(true);
    try {
      await onSubmit({
        description: description.trim(),
        value: parsedValue,
        type,
        personId,
      });
      setDescription('');
      setValue('');
      setType(TransactionType.Expense);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao cadastrar transação.');
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className="card form" onSubmit={handleSubmit}>
      <h3>Nova transação</h3>
      <div className="form-row">
        <label>
          Pessoa
          <select value={personId} onChange={(e) => setPersonId(e.target.value)}>
            <option value="">Selecione...</option>
            {people.map((p) => (
              <option key={p.id} value={p.id}>
                {p.name} {p.isMinor ? '(menor de idade)' : ''}
              </option>
            ))}
          </select>
        </label>
        <label>
          Descrição
          <input
            type="text"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="Ex: Supermercado"
          />
        </label>
        <label>
          Valor (R$)
          <input
            type="number"
            min={0}
            step="0.01"
            value={value}
            onChange={(e) => setValue(e.target.value)}
            placeholder="Ex: 150.00"
          />
        </label>
        <label>
          Tipo
          <select
            value={type}
            onChange={(e) => setType(Number(e.target.value) as TransactionType)}
          >
            <option value={TransactionType.Expense}>Despesa</option>
            <option value={TransactionType.Income} disabled={incomeDisabled}>
              Receita
            </option>
          </select>
        </label>
        <button type="submit" disabled={submitting}>
          {submitting ? 'Salvando...' : 'Adicionar'}
        </button>
      </div>
      {incomeDisabled && (
        <p className="hint-message">
          Essa pessoa é menor de idade: apenas despesas podem ser cadastradas.
        </p>
      )}
      {error && <p className="error-message">{error}</p>}
    </form>
  );
}
