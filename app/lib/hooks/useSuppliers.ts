"use client";

/* Supplier + driver dropdowns.
 *
 * The CoreApi has no single "suppliers with their contacts" call: the supplier
 * list comes from /Supplier/GetSuppliersCards, and a supplier's contact people
 * (the drivers) come from /Supplier/GetSupplierContactPersonsResponse one
 * supplier at a time. Every screen with the supplier → driver cascade uses this
 * hook so the fetch pattern lives in one place. */
import { useState, useEffect } from "react";
import { getSuppliers, getSupplierContacts } from "@/lib/services/suppliers";
import type { Option } from "@/components/ui/Combobox";

export interface SupplierOption { Id: number | string; Name: string }

/** All suppliers, for a dropdown. Loaded once. */
export function useSupplierOptions(onError?: (e: unknown) => void): SupplierOption[] {
  const [opts, setOpts] = useState<SupplierOption[]>([]);
  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        const res = await getSuppliers({ pageNo: 1, noOfItems: 500 });
        if (alive) setOpts(res.items.map((s) => ({ Id: s.id, Name: s.name })));
      } catch (e) {
        onError?.(e);
      }
    })();
    return () => { alive = false; };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return opts;
}

/** The chosen supplier's contact people (drivers). Refetches when it changes. */
export function useDriverOptions(supplierId: string | undefined, onError?: (e: unknown) => void): Option[] {
  const [opts, setOpts] = useState<Option[]>([]);
  useEffect(() => {
    if (!supplierId) { setOpts([]); return; }
    let alive = true;
    (async () => {
      try {
        const contacts = await getSupplierContacts(supplierId);
        if (alive) setOpts(contacts.map((c) => ({ value: c.id, label: c.name })));
      } catch (e) {
        onError?.(e);
      }
    })();
    return () => { alive = false; };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [supplierId]);
  return opts;
}
