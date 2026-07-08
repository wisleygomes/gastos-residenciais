import type { Person } from '../types';

interface PeopleListProps {
  people: Person[];
  onDelete: (id: string) => Promise<void>;
}

/**
 * Lista de pessoas cadastradas, com botão de remoção. Ao remover uma pessoa,
 * o backend também apaga em cascata todas as transações associadas a ela
 * (por isso o aviso exibido no botão de exclusão).
 */
export default function PeopleList({ people, onDelete }: PeopleListProps) {
  async function handleDelete(person: Person) {
    const confirmed = window.confirm(
      `Remover "${person.name}"? Todas as transações dessa pessoa também serão apagadas.`
    );
    if (!confirmed) return;
    await onDelete(person.id);
  }

  if (people.length === 0) {
    return <p className="empty-state">Nenhuma pessoa cadastrada ainda.</p>;
  }

  return (
    <table className="data-table">
      <thead>
        <tr>
          <th>Nome</th>
          <th>Idade</th>
          <th>Status</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        {people.map((person) => (
          <tr key={person.id}>
            <td>{person.name}</td>
            <td>{person.age}</td>
            <td>
              {person.isMinor ? (
                <span className="badge badge-warning">Menor de idade</span>
              ) : (
                <span className="badge badge-neutral">Maior de idade</span>
              )}
            </td>
            <td>
              <button className="btn-danger" onClick={() => handleDelete(person)}>
                Remover
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
