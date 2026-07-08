import { useState, FormEvent } from 'react';
import type { CreatePersonInput } from '../types';

interface PersonFormProps {
  onSubmit: (input: CreatePersonInput) => Promise<void>;
}

/**
 * Formulário controlado para cadastro de uma nova pessoa (nome + idade).
 * A validação de campos obrigatórios é feita aqui no cliente para dar
 * feedback rápido; a validação definitiva de regras de negócio sempre
 * acontece no backend.
 */
export default function PersonForm({ onSubmit }: PersonFormProps) {
  const [name, setName] = useState('');
  const [age, setAge] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError(null);

    const trimmedName = name.trim();
    const parsedAge = Number(age);

    if (!trimmedName) {
      setError('Informe o nome da pessoa.');
      return;
    }
    if (!Number.isInteger(parsedAge) || parsedAge < 0) {
      setError('Informe uma idade válida.');
      return;
    }

    setSubmitting(true);
    try {
      await onSubmit({ name: trimmedName, age: parsedAge });
      setName('');
      setAge('');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao cadastrar pessoa.');
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <form className="card form" onSubmit={handleSubmit}>
      <h3>Nova pessoa</h3>
      <div className="form-row">
        <label>
          Nome
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Ex: Maria Silva"
          />
        </label>
        <label>
          Idade
          <input
            type="number"
            min={0}
            value={age}
            onChange={(e) => setAge(e.target.value)}
            placeholder="Ex: 30"
          />
        </label>
        <button type="submit" disabled={submitting}>
          {submitting ? 'Salvando...' : 'Adicionar'}
        </button>
      </div>
      {error && <p className="error-message">{error}</p>}
    </form>
  );
}
